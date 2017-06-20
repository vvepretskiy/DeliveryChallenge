using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Repository
{
	public interface ISkillRepository : ICrudRepository<Skill>
	{
		IEnumerable<Skill> GetAllMatched(Employee employee);

		IEnumerable<Skill> GetAllUnmatched(Employee employee);

		IEnumerable<Skill> GetAllMatched(Delivery delivery);

		IEnumerable<Skill> GetAllUnmatched(Delivery delivery);
	}

	public class SkillRepository : BaseRepository, ISkillRepository
	{
		public SkillRepository(IModelDbContext context) : base(context)
		{
		}

		public IEnumerable<Skill> GetAll()
		{
			return _context.Skills;
		}

		public Skill Add(Skill item)
		{
			this._context.Skills.Add(item);
			_context.SaveChanges();
			return item;
		}

		public void Update(Skill item)
		{
			// Check there's not an object with same identifier already in context
			if (_context.Skills.Local.Select(x => x.Id == item.Id).Any())
			{
				throw new ApplicationException("Object already exists in context");
			}
			_context.Entry(item).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void Delete(Skill item)
		{
			_context.Entry(item).State = EntityState.Deleted;
			_context.SaveChanges();
		}

		public Skill Get(int id)
		{
			return _context.Skills.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Skill> GetAllMatched(Employee employee)
		{
			IEnumerable<int> addedSkillIds = employee.Skills
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedSkillIds = employee.Skills
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Skills.Include(x => x.Employees).Where(x =>
				addedSkillIds.Contains(x.Id) ||
				(!removedSkillIds.Contains(x.Id) && x.Employees.Any(y => y.Id == employee.Id)));
		}

		public IEnumerable<Skill> GetAllUnmatched(Employee employee)
		{
			IEnumerable<int> addedSkillIds = employee.Skills
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedSkillIds = employee.Skills
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Skills.Include(x => x.Employees).Where(x =>
				removedSkillIds.Contains(x.Id) ||
				(!addedSkillIds.Contains(x.Id) && x.Employees.All(y => y.Id != employee.Id)));
		}

		public IEnumerable<Skill> GetAllMatched(Delivery delivery)
		{
			IEnumerable<int> addedSkillIds = delivery.Skills
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedSkillIds = delivery.Skills
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Skills.Include(x => x.Deliveries).Where(x =>
				addedSkillIds.Contains(x.Id) ||
				(!removedSkillIds.Contains(x.Id) && x.Deliveries.Any(y => y.Id == delivery.Id)));
		}

		public IEnumerable<Skill> GetAllUnmatched(Delivery delivery)
		{
			IEnumerable<int> addedSkillIds = delivery.Skills
					.Where(x => x.EntityState == EntityState.Added).Select(x => x.Id).ToList();
			IEnumerable<int> removedSkillIds = delivery.Skills
					.Where(x => x.EntityState == EntityState.Deleted).Select(x => x.Id).ToList();

			return _context.Skills.Include(x => x.Deliveries).Where(x =>
				removedSkillIds.Contains(x.Id) ||
				(!addedSkillIds.Contains(x.Id) && x.Deliveries.All(y => y.Id != delivery.Id)));
		}
	}
}
