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
	public class DeliveryRepositoryTest
	{
		[TestMethod]
		public void GetAllTest()
		{
			var data = new List<Delivery>
			{
				new Delivery { Id = 0 },
				new Delivery { Id = 1 },
				new Delivery { Id = 2 },
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Delivery>>();
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Deliveries).Returns(mockSet.Object);

			var repository = new DeliveryRepository(mockContext.Object);
			var deliveries = repository.GetAll().ToArray();

			Assert.AreEqual(3, deliveries.Length);
			Assert.AreEqual(0, deliveries[0].Id);
			Assert.AreEqual(1, deliveries[1].Id);
			Assert.AreEqual(2, deliveries[2].Id);
		}

		[TestMethod]
		public void AddTest()
		{
			var mockSet = new Mock<DbSet<Delivery>>();
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.GetEnumerator()).Returns(new List<Delivery>().GetEnumerator());

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Deliveries).Returns(mockSet.Object);

			IDeliveryRepository repository = new DeliveryRepository(mockContext.Object);
			repository.Add(new Delivery());

			mockSet.Verify(m => m.Add(It.IsAny<Delivery>()), Times.Once());
			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void DeleteTest()
		{
			var data = new List<Delivery>().AsQueryable();

			var mockSet = new Mock<DbSet<Delivery>>();
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Deliveries).Returns(mockSet.Object);

			IDeliveryRepository repository = new DeliveryRepository(mockContext.Object);
			repository.Delete(new Delivery());

			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void GetTest()
		{
			var data = new List<Delivery>
			{
				new Delivery(),
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Delivery>>();
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Delivery>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Deliveries).Returns(mockSet.Object);

			IDeliveryRepository repository = new DeliveryRepository(mockContext.Object);
			Delivery delivery = repository.Get(0);

			Assert.AreEqual(delivery.Id, 0);
		}
	}
}
