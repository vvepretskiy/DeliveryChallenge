using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using DeliveryChallenge.Models.Entity;
using DeliveryChallenge.Models.Mapping;

namespace DeliveryChallenge.Models
{
	public interface IModelDbContext : IDisposable
	{ }

	[DbConfigurationType(typeof(DataConfiguration))]
	public class ModelDbContext : DbContext, IModelDbContext
	{
		public const string SQL_REMOVE_EMPLOYEE_SKILL = "DELETE FROM [dbo].[EmployeeSkill] WHERE EmployeeID = @EmployeeId AND SkillID = @SkillID";
		public const string SQL_ADD_EMPLOYEE_SKILL = "INSERT INTO [dbo].[EmployeeSkill](EmployeeID, SkillID) VALUES(@EmployeeId, @SkillID)";
		public const string SQL_REMOVE_DELIVERY_SKILL = "DELETE FROM [dbo].[DeliverySkill] WHERE DeliveryID = @DeliveryId AND SkillID = @SkillID";
		public const string SQL_ADD_DELIVERY_SKILL = "INSERT INTO [dbo].[DeliverySkill](DeliveryID, SkillID) VALUES(@DeliveryId, @SkillID)";
		public const string SQL_REMOVE_TEAM_MEMBER = "DELETE FROM [dbo].[TeamMember] WHERE EmployeeID = @EmployeeId AND DeliveryID = @DeliveryID";
		public const string SQL_ADD_TEAM_MEMBER = "INSERT INTO [dbo].[TeamMember](EmployeeID, DeliveryID) VALUES(@EmployeeId, @DeliveryID)";
		public const string UNMATCH_DELIVERY_MESSAGE = "Delivery skills cannot be matched to available skills";
		public const string UNMATCH_EMPLOYEE_MESSAGE = "Employee skills cannot be matched to available skills";

		public ModelDbContext()
		{
			Configuration.LazyLoadingEnabled = false;
			Configuration.ValidateOnSaveEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Mappings
			modelBuilder.Configurations.Add(new DeliveryDetailMapping());
			modelBuilder.Configurations.Add(new DeliveryMapping());
			modelBuilder.Configurations.Add(new DeliveryTypeMapping());
			modelBuilder.Configurations.Add(new EmployeeMapping());
			modelBuilder.Configurations.Add(new SkillMapping());

			base.OnModelCreating(modelBuilder);
		}

		public new void Dispose()
		{

		}

		public bool IsSkillMatch(IEnumerable<Skill> skills, IEnumerable<Delivery> deliveries)
		{
			IEnumerable<int> skillIds = skills
						.Where(x => x.EntityState != EntityState.Deleted).Select(x => x.Id).ToList();

			foreach (Delivery del in deliveries.Where(x => x.EntityState != EntityState.Deleted))
			{
				if (!Deliveries.Include(x => x.Skills).Any(x => x.Id == del.Id && x.Skills.Any(y => skillIds.Contains(y.Id))))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsSkillMatch(IEnumerable<Skill> skills, IEnumerable<Employee> employees)
		{
			IEnumerable<int> skillIds = skills
						.Where(x => x.EntityState != EntityState.Deleted).Select(x => x.Id).ToList();

			var empls = employees.Where(x => x.EntityState != EntityState.Deleted);

			foreach (Employee emp in empls)
			{
				if (!Employees.Include(x => x.Skills).Any(x => x.Id == emp.Id && x.Skills.Any(y => skillIds.Contains(y.Id))))
				{
					return false;
				}
			}

			return true;
		}

		protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
		{
			var result = base.ValidateEntity(entityEntry, items);

			if (entityEntry.State == EntityState.Modified)
			{
				var employee = entityEntry.Entity as Employee;

				if (employee != null && !IsSkillMatch(employee.Skills, employee.Deliveries))
				{
					result.ValidationErrors.Add(new DbValidationError("Deliveries", UNMATCH_DELIVERY_MESSAGE));
				}

				var delivery = entityEntry.Entity as Delivery;

				if (delivery != null && !IsSkillMatch(delivery.Skills, delivery.Employees))
				{
					result.ValidationErrors.Add(new DbValidationError("Employees", UNMATCH_EMPLOYEE_MESSAGE));
				}
			}
			
			return result;
		}

		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<Skill> Skills { get; set; }
		public virtual DbSet<Delivery> Deliveries { get; set; }
		public virtual DbSet<DeliveryType> DeliveryTypes { get; set; }
		public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }
	}
}
