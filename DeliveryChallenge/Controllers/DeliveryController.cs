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
	public class DeliveryController : Controller
	{
		private readonly IDeliveryRepository _deliveryRepository;

		public DeliveryController(IDeliveryRepository deliveryRepository)
		{
			_deliveryRepository = deliveryRepository;
		}

		public ActionResult Index()
		{
			return View();
		}

		private IEnumerable<Delivery> GetLimit(IEnumerable<Delivery> items, int rows, int page)
		{
			return items.Skip((page - 1) * rows).Take(rows);
		}

		public JsonResult Get(int rows = 10, int page = 1)
		{
			var data = _deliveryRepository.GetAll();

			GridViewModel viewModel = new GridViewModel(data.Count(), rows);

			viewModel.Data = GetLimit(data, rows, page)
				.Select(
					x =>
						new Delivery
						{
							Skills = x.Skills
										.Take(rows)
										.Select(z => new Skill { Id = z.Id, Name = z.Name }).ToList(),
							Id = x.Id,
							Name = x.Name,
							Type = new DeliveryType { Id = x.Type.Id, Name = x.Type.Name},
							Employees = x.Employees
											.Take(rows)
											.Select(y => new Employee { Id = y.Id, FirstName = y.FirstName, SecondName = y.SecondName }).ToList()
						});

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetEmployeeDeliveries(Employee data, int rows = 10, int page = 1)
		{
			var items = _deliveryRepository.GetAllMatched(data);

			GridViewModel viewModel = new GridViewModel(items.Count(), rows)
			{
				Data = GetLimit(items, rows, page).Select(x => new Delivery { Id = x.Id, Name = x.Name })
			};

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetAvailableEmployeeDeliveries(Employee data, int rows = 10, int page = 1)
		{
			var items = _deliveryRepository.GetAllUnmatched(data);

			GridViewModel viewModel = new GridViewModel(items.Count(), rows)
			{
				Data = GetLimit(items, rows, page).Select(x => new Delivery { Id = x.Id, Name = x.Name })
			};

			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Edit(Delivery data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_deliveryRepository.Update(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Add(Delivery data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_deliveryRepository.Add(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Data = data, Message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Remove(Delivery data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_deliveryRepository.Delete(data);
			}
			else
			{
				message = Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}
