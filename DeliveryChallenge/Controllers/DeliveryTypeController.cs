using System.Web.Mvc;
using DeliveryChallenge.Models.Repository;

namespace DeliveryChallenge.Controllers
{
	public class DeliveryTypeController : Controller
	{
		private readonly IDeliveryTypeRepository _deliveryTypeRepository;

		public DeliveryTypeController(IDeliveryTypeRepository deliveryTypeRepository)
		{
			_deliveryTypeRepository = deliveryTypeRepository;
		}

		public JsonResult Get()
		{
			var data = _deliveryTypeRepository.GetAll();
			return Json(data, JsonRequestBehavior.AllowGet);
		}
	}
}
