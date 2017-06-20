using System.Collections.Generic;

namespace DeliveryChallenge.Models.Entity
{
	public class DeliveryType
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public virtual ICollection<Delivery> Deliveries { get; set; }
	}
}
