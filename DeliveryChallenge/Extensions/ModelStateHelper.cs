using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DeliveryChallenge.Extensions
{
	public static class ModelStateHelper
	{
		public static IEnumerable<string> Errors(this ModelStateDictionary modelState)
		{
			if (!modelState.IsValid)
			{
				return modelState.Values.SelectMany(e => e.Errors.Select(x => x.ErrorMessage + System.Environment.NewLine));
			}
			return new List<string>();
		}
	}
}
