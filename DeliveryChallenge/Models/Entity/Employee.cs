using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DeliveryChallenge.Models.Entity
{
	public class Employee
	{
		public Employee()
		{
			Skills = new HashSet<Skill>();
			Deliveries = new HashSet<Delivery>();
		}

		[Required(ErrorMessage = "Id is required")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Editable(false)]
		public int Id { get; set; }

		[Required(ErrorMessage = "FirstName is required")]
		[MaxLength(50, ErrorMessage = "FirstName cannot have more 50 characters")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "SecondName is required")]
		[MaxLength(50, ErrorMessage = "SecondName cannot have more 50 characters")]
		public string SecondName { get; set; }

		public virtual ICollection<Delivery> Deliveries { get; set; }

		public virtual ICollection<Skill> Skills { get; set; }

		[NotMapped]
		public EntityState EntityState { get; set; }
	}
}
