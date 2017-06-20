using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace DeliveryChallenge.Exception
{
	public class FormattedException : System.Exception
	{
		public FormattedException(System.Exception innerException) :
		base(null, innerException)
		{
		}

		public override string Message
		{
			get
			{
				var ex = InnerException as DbEntityValidationException;
				if (ex != null)
				{
					return GetMessage(ex.EntityValidationErrors);
				}

				var ex1 = InnerException as System.Data.Entity.Validation.DbEntityValidationException;

				if (ex1 != null)
				{
					return GetMessage(ex1.EntityValidationErrors);
				}

				return base.Message;
			}
		}

		private string GetMessage(IEnumerable<DbEntityValidationResult> errors)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var error in errors.SelectMany(x => x.ValidationErrors))
			{
				sb.AppendLine(error.ErrorMessage);
			}
			return sb.ToString();
		}
	}
}
