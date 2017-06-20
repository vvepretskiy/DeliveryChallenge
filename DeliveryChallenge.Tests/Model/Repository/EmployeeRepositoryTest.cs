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
	public class EmployeeRepositoryTest
	{
		[TestMethod]
		public void GetAllTest()
		{
			var data = new List<Employee>
			{
				new Employee { Id = 0 },
				new Employee { Id = 1 },
				new Employee { Id = 2 },
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Employee>>();
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Employees).Returns(mockSet.Object);

			var repository = new EmployeeRepository(mockContext.Object);
			var Employees = repository.GetAll().ToArray();

			Assert.AreEqual(3, Employees.Length);
			Assert.AreEqual(0, Employees[0].Id);
			Assert.AreEqual(1, Employees[1].Id);
			Assert.AreEqual(2, Employees[2].Id);
		}

		[TestMethod]
		public void AddTest()
		{
			var mockSet = new Mock<DbSet<Employee>>();
			mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(new List<Employee>().GetEnumerator());

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Employees).Returns(mockSet.Object);

			IEmployeeRepository repository = new EmployeeRepository(mockContext.Object);
			repository.Add(new Employee());

			mockSet.Verify(m => m.Add(It.IsAny<Employee>()), Times.Once());
			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void DeleteTest()
		{
			var data = new List<Employee>().AsQueryable();

			var mockSet = new Mock<DbSet<Employee>>();
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Employees).Returns(mockSet.Object);

			IEmployeeRepository repository = new EmployeeRepository(mockContext.Object);
			repository.Delete(new Employee());

			mockContext.Verify(m => m.SaveChanges(), Times.Once());
		}

		[TestMethod]
		public void GetTest()
		{
			var data = new List<Employee>
			{
				new Employee(),
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Employee>>();
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

			var mockContext = new Mock<ModelDbContext>();
			mockContext.Setup(m => m.Employees).Returns(mockSet.Object);

			IEmployeeRepository repository = new EmployeeRepository(mockContext.Object);
			Employee Employee = repository.Get(0);

			Assert.AreEqual(Employee.Id, 0);
		}
	}
}
