using System.Collections.Generic;

namespace DeliveryChallenge.Exception
{
	public class DbEntityValidationException : System.Data.Entity.Validation.DbEntityValidationException
	{
		public new IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> EntityValidationErrors { get; set; }
	}
}
