using System;
using System.Net;
using System.Web.Mvc;

namespace DeliveryChallenge.App_Start
{
	public class ErrorHandler : HandleErrorAttribute
	{
		private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Startup));

		public override void OnException(ExceptionContext filterContext)
		{
			var statusCode = (int)HttpStatusCode.InternalServerError;
			if (filterContext.Exception is UnauthorizedAccessException)
			{
				//to prevent login prompt in IIS
				// which will appear when returning 401.
				statusCode = (int)HttpStatusCode.Forbidden;
			}

			// log4net
			_logger.Error("Uncaught exception", filterContext.Exception);

			filterContext.Result = new JsonResult
			{
				Data = new {Success = false, filterContext.Exception.Message},
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = statusCode;

			base.OnException(filterContext);
		}
	}
}
