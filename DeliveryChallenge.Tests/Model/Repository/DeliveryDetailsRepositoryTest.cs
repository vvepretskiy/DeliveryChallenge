using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Entity;
using DeliveryChallenge.Models.Repository;

namespace DeliveryChallenge.Tests.Model.Repository
{
	[TestClass]
	public class DeliveryDetailsRepositoryTest
	{
		[TestMethod]
		public void GetAllTest()
		{
			var data = new List<DeliveryDetail>
			{
				new DeliveryDetail { Id = 0 },
				new DeliveryDetail { Id = 1 },
				new DeliveryDetail { Id = 2 },
			}.AsQueryable();

			var mockSet = new Mock<DbSet<DeliveryDetail>>();
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.DeliveryDetails).Returns(mockSet.Object);

			var repository = new DeliveryDetailRepository(mockContext.Object);
			var deliveryDetails = repository.GetAll().ToArray();

			Assert.AreEqual(3, deliveryDetails.Length);
			Assert.AreEqual(0, deliveryDetails[0].Id);
			Assert.AreEqual(1, deliveryDetails[1].Id);
			Assert.AreEqual(2, deliveryDetails[2].Id);
		}

		[TestMethod]
		public void AddTest()
		{
			var mockSet = new Mock<DbSet<DeliveryDetail>>();
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.GetEnumerator()).Returns(new List<DeliveryDetail>().GetEnumerator());

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.DeliveryDetails).Returns(mockSet.Object);

			var repository = new DeliveryDetailRepository(mockContext.Object);
			repository.Add(new DeliveryDetail());

			mockSet.Verify(m => m.Add(It.IsAny<DeliveryDetail>()), Times.Once());
			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void DeleteTest()
		{
			var data = new List<DeliveryDetail>().AsQueryable();

			var mockSet = new Mock<DbSet<DeliveryDetail>>();
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.DeliveryDetails).Returns(mockSet.Object);

			var repository = new DeliveryDetailRepository(mockContext.Object);
			repository.Delete(new DeliveryDetail());

			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void GetTest()
		{
			var data = new List<DeliveryDetail>
			{
				new DeliveryDetail(),
			}.AsQueryable();

			var mockSet = new Mock<DbSet<DeliveryDetail>>();
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<DeliveryDetail>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.DeliveryDetails).Returns(mockSet.Object);

			var repository = new DeliveryDetailRepository(mockContext.Object);
			DeliveryDetail deliveryDetail = repository.Get(0);

			Assert.AreEqual(deliveryDetail.Id, 0);
		}
	}
}
