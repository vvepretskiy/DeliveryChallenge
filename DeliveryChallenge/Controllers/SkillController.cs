using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DeliveryChallenge.Extensions;
using DeliveryChallenge.Models;
using DeliveryChallenge.Models.Entity;
using DeliveryChallenge.Models.Repository;

namespace DeliveryChallenge.Controllers
{
	public class SkillController : Controller
	{
		private readonly ISkillRepository _skillRepository;

		public SkillController(ISkillRepository skillRepository)
		{
			_skillRepository = skillRepository;
		}

		public ActionResult Index()
		{
			return View();
		}

		public JsonResult Get(int rows = 10, int page = 1)
		{
			return Get(_skillRepository.GetAll(), rows, page);
		}

		[HttpPost]
		public JsonResult GetEmployeeSkills(Employee data, int rows = 10, int page = 1)
		{
			return Get(_skillRepository.GetAllMatched(data), rows, page);
		}

		[HttpPost]
		public JsonResult GetAvailableEmployeeSkills(Employee data, int rows = 10, int page = 1)
		{
			return Get(_skillRepository.GetAllUnmatched(data), rows, page);
		}

		[HttpPost]
		public JsonResult GetDeliverySkills(Delivery data, int rows = 10, int page = 1)
		{
			return Get(_skillRepository.GetAllMatched(data), rows, page);
		}

		[HttpPost]
		public JsonResult GetAvailableDeliverySkills(Delivery data, int rows = 10, int page = 1)
		{
			return Get(_skillRepository.GetAllUnmatched(data), rows, page);
		}

		private JsonResult Get(IEnumerable<Skill> data, int rows = 10, int page = 1)
		{
			GridViewModel viewModel = new GridViewModel { TotalRowCount = data.Count() };

			int totalPages = 1;
			if (rows > 0)
			{
				totalPages = viewModel.TotalRowCount / rows;
				if (viewModel.TotalRowCount % rows != 0)
					totalPages += 1;
			}

			viewModel.TotalPageCount = totalPages;
			viewModel.Data = data
				.Skip((page - 1) * rows)
				.Take(rows)
				.Select(x => new Skill { Id = x.Id, Name = x.Name });
			return Json(viewModel, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Edit(Skill data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_skillRepository.Update(data);
			}
			else
			{
				message = String.Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Add(Skill data)
		{
			string message = null;
			if (ModelState.IsValid)
			{
				_skillRepository.Add(data);
			}
			else
			{
				message = String.Join(Environment.NewLine, ModelState.Errors());
			}

			return Json(new { Success = ModelState.IsValid, Data = data, Message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}
