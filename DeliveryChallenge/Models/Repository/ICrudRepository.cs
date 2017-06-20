using System.Collections.Generic;

namespace DeliveryChallenge.Models.Repository
{
	public interface ICrudRepository<T>
	{
		IEnumerable<T> GetAll();
		T Add(T item);
		void Update(T item);
		void Delete(T item);
		T Get(int id);
	}
}
