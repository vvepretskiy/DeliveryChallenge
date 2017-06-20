using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using DeliveryChallenge.Exception;
using DeliveryChallenge.Models.Entity;
using DbEntityValidationException = DeliveryChallenge.Exception.DbEntityValidationException;

namespace DeliveryChallenge.Models.Repository
{
	public interface IEmployeeRepository : ICrudRepository<Employee>
	{
		IEnumerable<Employee> GetAllMatched(Delivery delivery);

		IEnumerable<Employee> GetAllUnmatched(Delivery delivery);
	}

	public class EmployeeRepository : BaseRepository, IEmployeeRepository
	{
		public EmployeeRepository(IModelDbContext context) : base(context)
		{
		}

		public IEnumerable<Employee> GetAll()
		{
			return _context.Employees.Include(x => x.Skills).Include(x => x.Deliveries);
		}

		public Employee Add(Employee item)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (!_context.IsSkillMatch(item.Skills, item.Deliveries))
					{
						throw new DbEntityValidationException
						{
							EntityValidationErrors = new Collection<DbEntityValidationResult>
							{
								new DbEntityValidationResult(_context.Entry(item),
									new[] {new DbValidationError(null, UNMATCH_DELIVERY_MESSAGE)})
							}
						};
					}

					IList<Skill> skills = item.Skills.Where(x => x.EntityState == EntityState.Added).ToList();
					IList<Delivery> deliveries = item.Deliveries.Where(x => x.EntityState == EntityState.Added).ToList();
					item.Skills.Clear();
					item.Deliveries.Clear();

					this._context.Employees.Add(item);

					_context.SaveChanges();

					UpdateDeliveryAndSkill(item, skills, deliveries);

					dbContextTransaction.Commit();
				}
				catch (System.Exception ex)
				{
					dbContextTransaction.Rollback(); //Required according to MSDN article 
					throw new FormattedException(ex); //Not in MSDN article, but recommended so the exception still bubbles up
				}
			}

			return item;
		}

		private void UpdateDeliveryAndSkill(Employee employee, IEnumerable<Skill> skills, IEnumerable<Delivery> deliveries)
		{
			foreach (Skill skill in skills)
			{
				if (skill.EntityState == EntityState.Added)
				{
					_context.Database.ExecuteSqlCommand(SQL_ADD_EMPLOYEE_SKILL,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@SkillID", skill.Id));
				}
				else if (skill.EntityState == EntityState.Deleted)
				{
					_context.Database.ExecuteSqlCommand(SQL_REMOVE_EMPLOYEE_SKILL,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@SkillID", skill.Id));
				}
			}

			_context.SaveChanges();

			foreach (Delivery delivery in deliveries)
			{
				if (delivery.EntityState == EntityState.Added)
				{
					_context.Database.ExecuteSqlCommand(SQL_ADD_TEAM_MEMBER,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@DeliveryID", delivery.Id));
				}
				else if (delivery.EntityState == EntityState.Deleted)
				{
					_context.Database.ExecuteSqlCommand(SQL_REMOVE_TEAM_MEMBER,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@DeliveryID", delivery.Id));
				}
			}

			_context.SaveChanges();
		}

		public void Update(Employee item)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{
				try
				{
					// Check there's not an object with same identifier already in context
					if (_context.Employees.Local.Select(x => x.Id == item.Id).Any())
					{
						throw new ApplicationException("Object already exists in context");
					}

					_context.Entry(item).State = EntityState.Modified;

					UpdateDeliveryAndSkill(item, item.Skills, item.Deliveries);
					dbContextTransaction.Commit();
				}
				catch (System.Exception ex)
				{
					dbContextTransaction.Rollback(); //Required according to MSDN article 
					throw new FormattedException(ex); //Not in MSDN article, but recommended so the exception still bubbles up
				}
			}
		}

		public void Delete(Employee item)
		{
			this._context.Entry(item).State = EntityState.Deleted;
			_context.SaveChanges();
		}

		public Employee Get(int id)
		{
			return _context.Employees.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Employee> GetAllMatched(Delivery delivery)
		{
			IEnumerable<int> addedDeliveryIds = delivery.Employees
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedDeliveryIds = delivery.Employees
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Employees.Include(x => x.Deliveries)
				.Where(x =>
				addedDeliveryIds.Contains(x.Id) ||
				(!removedDeliveryIds.Contains(x.Id) && x.Deliveries.Any(y => y.Id == delivery.Id)));
		}

		public IEnumerable<Employee> GetAllUnmatched(Delivery delivery)
		{
			IEnumerable<int> addedDeliveryIds = delivery.Employees
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedDeliveryIds = delivery.Employees
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Employees.Include(x => x.Deliveries)
				.Where(x =>
				removedDeliveryIds.Contains(x.Id) ||
				(!addedDeliveryIds.Contains(x.Id) && x.Deliveries.All(y => y.Id != delivery.Id)));
		}
	}
}
