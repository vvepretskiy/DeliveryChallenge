using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DeliveryChallenge.Models.Entity
{
	public class Delivery
	{
		public Delivery()
		{
			Skills = new HashSet<Skill>();
			Employees = new HashSet<Employee>();
			Details = new HashSet<DeliveryDetail>();
		}

		[Required(ErrorMessage = "Id is required")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Editable(false)]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[MaxLength(255, ErrorMessage = "FirstName cannot have more 50 characters")]
		public string Name { get; set; }

		public DeliveryType Type { get; set; }

		public int DeliveryTypeId { get; set; }

		public virtual ICollection<Employee> Employees { get; set; }

		public virtual ICollection<DeliveryDetail> Details { get; set; }

		public virtual ICollection<Skill> Skills { get; set; }

		[NotMapped]
		public EntityState EntityState { get; set; }
	}
}
