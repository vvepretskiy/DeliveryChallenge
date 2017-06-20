using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using DeliveryChallenge.Attributes;

namespace DeliveryChallenge.Models.Entity
{
	public class Skill
	{
		[Required(ErrorMessage = "Id is required")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Editable(false)]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[MaxLength(50, ErrorMessage = "Name cannot have more 50 characters")]
		[Index(IsUnique = true)]
		[Unique(ErrorMessage = "This name already exists", TargetModelType = typeof(Skill))]
		public string Name { get; set; }

		public virtual ICollection<Employee> Employees { get; set; }

		public virtual ICollection<Delivery> Deliveries { get; set; }

		[NotMapped]
		public EntityState EntityState { get; set; }
	}
}
