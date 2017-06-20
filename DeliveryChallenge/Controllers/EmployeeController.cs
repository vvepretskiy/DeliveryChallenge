using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DeliveryChallenge.Extensions;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Entity;
using DeliveryChallenge.Models.Repository;
using static System.String;

namespace DeliveryChallenge.Controllers
{
    public class EmployeeController : Controller
    {
	    private readonly IEmployeeRepository _employeeRepository;

		public EmployeeController(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public ActionResult Index()
		{
			return View();
		}

		private IEnumerable<Employee> GetLimit(IEnumerable<Employee> items, int rows, int page)
		{
			return items.Skip((page - 1) * rows).Take(rows);
		}

		public JsonResult Get(int rows = 10, int page = 1)
		{
			var data = _employeeRepository.GetAll();

			GridViewModel viewModel = new GridViewModel(data.Count(), rows);

			viewModel.Data = GetLimit(data, rows, page)
				.Select(
					x =>
						new Employee
						{
							Skills = x.Skills
										.Take(rows)
										.Select(z => new Skill { Id = z.Id, Name = z.Name }).ToList(),
							Id = x.Id,
							FirstName = x.FirstName,
							SecondName = x.SecondName,
							Deliveries = x.Deliveries
											.Take(rows)
											.Select(y => new Delivery { Id = y.Id, Name = y.Name }).ToList()
						});

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetDeliveryEmployees(Delivery data, int rows = 10, int page = 1)
		{
			var items = _employeeRepository.GetAllMatched(data);

			GridViewModel viewModel = new GridViewModel(items.Count(), rows)
			{
				Data = GetLimit(items, rows, page).Select(x => new Employee { Id = x.Id, FirstName = x.FirstName, SecondName = x.SecondName})
			};

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetAvailableDeliveryEmployee(Delivery data, int rows = 10, int page = 1)
		{
			var items = _employeeRepository.GetAllUnmatched(data);

			GridViewModel viewModel = new GridViewModel(items.Count(), rows)
			{
				Data = GetLimit(items, rows, page).Select(x => new Employee { Id = x.Id, FirstName = x.FirstName, SecondName = x.SecondName})
			};

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Edit(Employee data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_employeeRepository.Update(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Add(Employee data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_employeeRepository.Add(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Data = data, Message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Remove(Employee data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_employeeRepository.Delete(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}
