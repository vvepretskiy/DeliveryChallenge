using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Tests.Model.Database
{
	[TestClass]
	public class ModelDbContextTest
	{
		[TestMethod]
		public void SkillTest()
		{
			System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<ModelDbContext>());
			using (var context = new ModelDbContext())
			{
				context.Database.Create();

				Skill skill = new Skill() { Name = "Test"};
				context.Entry(skill).State = EntityState.Added;
				context.SaveChanges();

				Assert.AreEqual(skill.Id, 1);

				var addedSkill = context.Skills.FirstOrDefault();

				Assert.AreEqual(addedSkill, skill);

				skill.Name = "Update";
				context.Entry(skill).State = EntityState.Modified;
				context.SaveChanges();

				context.Entry(skill).State = EntityState.Deleted;
				context.SaveChanges();
			}
		}

		[TestMethod]
		public void DeliveryTest()
		{
			//System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<ModelDbContext>());
			//using (var context = new ModelDbContext())
			//{
			//	context.Database.Create();

			//	var skill = new Skill() {Name = "Skill0"};

			//	Delivery delivery = new Delivery()
			//	{
			//		Name = "Test",
			//		Type = new DeliveryType() {Name = "Test"},
			//		Skills = new List<Skill>() { skill }
			//	};

			//	delivery.Employees = new List<Employee>()
			//	{
			//		new Employee()
			//		{
			//			FirstName = "FirstName1", SecondName = "SecondName1",
			//			Deliveries = new List<Delivery>() { delivery },
			//			Skills = new List<Skill>() { skill }
			//		}
			//	};

			//	delivery.Details = new List<DeliveryDetail>()
			//	{
			//		//new DeliveryDetail() {Key = "Key0", Value = "Value0", Delivery = delivery}
			//		new DeliveryDetail() {Key = "Key0", Value = "Value0", DeliveryId = delivery.Id}
			//	};

			//	context.Entry(delivery).State = EntityState.Added;
			//	context.SaveChanges();

			//	Assert.AreEqual(delivery.Id, 1);

			//	delivery.Employees.Clear();
			//	delivery.Skills.Clear();
			//	context.Entry(delivery).State = EntityState.Modified;
			//	context.SaveChanges();

			//	//context.Entry(delivery.Details.FirstOrDefault()).State = EntityState.Deleted;
			//	context.Entry(delivery).State = EntityState.Deleted;
			//	context.SaveChanges();
			//}
		}

		[TestCleanup]
		public void Dispose()
		{
			System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<ModelDbContext>());
			System.Data.Entity.Database.Delete("ModelDbContext");
		}
	}
}
