using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Linq.Dynamic;
using DeliveryChallenge.Models;

namespace DeliveryChallenge.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class Unique : ValidationAttribute
	{
		public Type TargetModelType { get; set; }

		public string TargetPropertyName { get; set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			return (TargetModelType == null || string.IsNullOrEmpty(TargetPropertyName)) ? DirectlyValid(value, validationContext) : ViewModelValid(value, validationContext);
		}

		private ValidationResult DirectlyValid(object value, ValidationContext validationContext)
		{
			using (ModelDbContext db = new ModelDbContext())
			{
				var name = GetName(validationContext);

				PropertyInfo idProp = validationContext.ObjectInstance.GetType().GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0);

				int id = (int)idProp.GetValue(validationContext.ObjectInstance, null);

				Type entityType = validationContext.ObjectType;


				var result = db.Set(entityType).Where(name + "==@0", value);
				int count = 0;

				if (id > 0)
				{
					result = result.Where(idProp.Name + "<>@0", id);
				}

				count = result.Count();

				if (count == 0)
					return ValidationResult.Success;
				return new ValidationResult(ErrorMessageString);
			}
		}

		private string GetName(ValidationContext validationContext)
		{
			var name = validationContext.MemberName;

			if (string.IsNullOrEmpty(name))
			{
				var displayName = validationContext.DisplayName;

				var prop = validationContext.ObjectInstance.GetType().GetProperty(displayName);

				if (prop != null)
				{
					name = prop.Name;
				}
				else
				{
					var props = validationContext.ObjectInstance.GetType().GetProperties().Where(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(DisplayAttribute)) > 0).ToList();

					foreach (PropertyInfo prp in props)
					{
						var attr = prp.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(DisplayAttribute));

						var val = attr.NamedArguments.FirstOrDefault(p => p.MemberName == "Name").TypedValue.Value;

						if (val.Equals(displayName))
						{
							name = prp.Name;
							break;
						}
					}
				}
			}

			return name;
		}

		private ValidationResult ViewModelValid(object value, ValidationContext validationContext)
		{
			using (ModelDbContext db = new ModelDbContext())
			{
				var name = TargetPropertyName;

				PropertyInfo idProp = TargetModelType.GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0) ?? TargetModelType.GetProperties().FirstOrDefault();

				int Id = (int)validationContext.ObjectInstance.GetType().GetProperty(idProp.Name).GetValue(validationContext.ObjectInstance, null);

				//int Id = (int)IdProp.GetValue(validationContext.ObjectInstance, null);

				Type entityType = TargetModelType;

				var result = db.Set(entityType).Where(name + "==@0", value);
				int count = 0;

				if (Id > 0)
				{
					result = result.Where(idProp.Name + "<>@0", Id);
				}

				count = result.Count();

				if (count == 0)
					return ValidationResult.Success;
				return new ValidationResult(ErrorMessageString);
			}
		}
	}
}
