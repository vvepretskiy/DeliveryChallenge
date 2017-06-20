using System.Web.Mvc;
using DeliveryChallenge.App_Start;

namespace DeliveryChallenge
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ErrorHandler(), 1);
		}
	}
}
