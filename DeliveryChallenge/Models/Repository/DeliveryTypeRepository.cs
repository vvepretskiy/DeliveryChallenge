using System;
using System.Collections.Generic;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Repository
{
	public interface IDeliveryTypeRepository : ICrudRepository<DeliveryType>
	{}

	public class DeliveryTypeRepository : BaseRepository, IDeliveryTypeRepository
	{
		public DeliveryTypeRepository(IModelDbContext context) : base(context)
		{}

		public IEnumerable<DeliveryType> GetAll()
		{
			return _context.DeliveryTypes;
		}

		public DeliveryType Add(DeliveryType item)
		{
			throw new NotImplementedException();
		}

		public void Update(DeliveryType item)
		{
			throw new NotImplementedException();
		}

		public void Delete(DeliveryType item)
		{
			throw new NotImplementedException();
		}

		public DeliveryType Get(int id)
		{
			throw new NotImplementedException();
		}
	}
}
