using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeliveryChallenge.Models;

namespace DeliveryChallenge.Tests.Model
{
	[TestClass]
	public class EmployeeTest
	{
		[TestMethod]
		public void LoadEmployees()
		{
			using (var context = new ModelDbContext())
			{
				var item = context.Employees.ToList();
			}
		}
	}
}
