using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Entity;
using DeliveryChallenge.Models.Repository;

namespace DeliveryChallenge.Tests.Model.Repository
{
	[TestClass]
	public class SkillRepositoryTest
	{
		[TestMethod]
		public void GetAllTest()
		{
			var data = new List<Skill>
			{
				new Skill { Id = 0},
				new Skill { Id = 1},
				new Skill { Id = 2},
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Skill>>();
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Skills).Returns(mockSet.Object);

			var repository = new SkillRepository(mockContext.Object);
			var skills = repository.GetAll().ToArray();

			Assert.AreEqual(3, skills.Length);
			Assert.AreEqual(0, skills[0].Id);
			Assert.AreEqual(1, skills[1].Id);
			Assert.AreEqual(2, skills[2].Id);
		}

		[TestMethod]
		public void AddTest()
		{
			var mockSet = new Mock<DbSet<Skill>>();
			mockSet.As<IQueryable<Skill>>().Setup(m => m.GetEnumerator()).Returns(new List<Skill>().GetEnumerator());

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Skills).Returns(mockSet.Object);

			ISkillRepository repository = new SkillRepository(mockContext.Object);
			repository.Add(new Skill());

			mockSet.Verify(m => m.Add(It.IsAny<Skill>()), Times.Once());
			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void DeleteTest()
		{
			var data = new List<Skill>().AsQueryable();

			var mockSet = new Mock<DbSet<Skill>>();
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Skills).Returns(mockSet.Object);

			ISkillRepository repository = new SkillRepository(mockContext.Object);
			repository.Delete(new Skill());

			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void GetTest()
		{
			var data = new List<Skill>
			{
				new Skill(),
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Skill>>();
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Skill>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Skills).Returns(mockSet.Object);

			ISkillRepository repository = new SkillRepository(mockContext.Object);
			Skill skill = repository.Get(0);

			Assert.AreEqual(skill.Id, 0);
		}
	}
}
