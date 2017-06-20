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
	public interface IDeliveryRepository : ICrudRepository<Delivery>
	{
		IEnumerable<Delivery> GetAllMatched(Employee employee);

		IEnumerable<Delivery> GetAllUnmatched(Employee employee);
	}

	public class DeliveryRepository : BaseRepository, IDeliveryRepository
	{
		public DeliveryRepository(IModelDbContext context) : base(context)
		{
		}

		public IEnumerable<Delivery> GetAll()
		{
			return _context.Deliveries.Include(x => x.Type).Include(x => x.Employees).Include(x => x.Skills);
		}

		public Delivery Add(Delivery item)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (!_context.IsSkillMatch(item.Skills, item.Employees))
						throw new DbEntityValidationException
						{
							EntityValidationErrors = new Collection<DbEntityValidationResult>
							{
								new DbEntityValidationResult(_context.Entry(item),
									new[] {new DbValidationError(null, UNMATCH_EMPLOYEE_MESSAGE)})
							}
						};

					IList<Skill> skills = item.Skills.Where(x => x.EntityState == EntityState.Added).ToList();
					IList<Employee> deliveries = item.Employees.Where(x => x.EntityState == EntityState.Added).ToList();
					item.Skills.Clear();
					item.Employees.Clear();
					item.DeliveryTypeId = item.Type.Id;
					item.Type = null;

					this._context.Deliveries.Add(item);

					_context.SaveChanges();

					UpdateEmployeeAndSkill(item, skills, deliveries);

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

		private void UpdateEmployeeAndSkill(Delivery delivery, IEnumerable<Skill> skills, IEnumerable<Employee> employees)
		{
			foreach (Skill skill in skills)
			{
				if (skill.EntityState == EntityState.Added)
				{
					_context.Database.ExecuteSqlCommand(SQL_ADD_DELIVERY_SKILL,
						new SqlParameter("@DeliveryID", delivery.Id),
						new SqlParameter("@SkillID", skill.Id));
				}
				else if (skill.EntityState == EntityState.Deleted)
				{
					_context.Database.ExecuteSqlCommand(SQL_REMOVE_DELIVERY_SKILL,
						new SqlParameter("@DeliveryID", delivery.Id),
						new SqlParameter("@SkillID", skill.Id));
				}
			}

			_context.SaveChanges();

			foreach (Employee employee in employees)
			{
				if (employee.EntityState == EntityState.Added)
				{
					_context.Database.ExecuteSqlCommand(SQL_ADD_TEAM_MEMBER,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@DeliveryID", delivery.Id));
				}
				else if (employee.EntityState == EntityState.Deleted)
				{
					_context.Database.ExecuteSqlCommand(SQL_REMOVE_TEAM_MEMBER,
						new SqlParameter("@EmployeeID", employee.Id),
						new SqlParameter("@DeliveryID", delivery.Id));
				}
			}

			_context.SaveChanges();
		}

		public void Update(Delivery item)
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

					item.DeliveryTypeId = item.Type.Id;
					_context.Entry(item).State = EntityState.Modified;

					UpdateEmployeeAndSkill(item, item.Skills, item.Employees);
					dbContextTransaction.Commit();
				}
				catch (System.Exception ex)
				{
					dbContextTransaction.Rollback(); //Required according to MSDN article 
					throw new FormattedException(ex); //Not in MSDN article, but recommended so the exception still bubbles up
				}
			}
		}

		public void Delete(Delivery item)
		{
			var del = Get(item.Id);
			this._context.Deliveries.Remove(del);
			_context.SaveChanges();
		}

		public Delivery Get(int id)
		{
			return _context.Deliveries.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Delivery> GetAllMatched(Employee employee)
		{
			IEnumerable<int> addedDeliveryIds = employee.Deliveries
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedDeliveryIds = employee.Deliveries
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Deliveries
				.Include(x => x.Employees)
				.Where(x =>
				addedDeliveryIds.Contains(x.Id) ||
				(!removedDeliveryIds.Contains(x.Id) && x.Employees.Any(y => y.Id == employee.Id)));
		}

		public IEnumerable<Delivery> GetAllUnmatched(Employee employee)
		{
			IEnumerable<int> addedDeliveryIds = employee.Deliveries
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedDeliveryIds = employee.Deliveries
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Deliveries
				.Include(x => x.Employees)
				.Where(x =>
				removedDeliveryIds.Contains(x.Id) ||
				(!addedDeliveryIds.Contains(x.Id) && x.Employees.All(y => y.Id != employee.Id)));
		}
	}
}
