using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Repository
{
	public interface IDeliveryDetailRepository : ICrudRepository<DeliveryDetail>
	{ }

	public class DeliveryDetailRepository : BaseRepository, IDeliveryDetailRepository
	{
		public DeliveryDetailRepository(IModelDbContext context) : base(context)
		{
		}

		public IEnumerable<DeliveryDetail> GetAll()
		{
			return _context.DeliveryDetails;
		}

		public DeliveryDetail Add(DeliveryDetail item)
		{
			this._context.DeliveryDetails.Add(item);
			_context.SaveChanges();
			return item;
		}

		public void Update(DeliveryDetail item)
		{
			// Check there's not an object with same identifier already in context
			if (_context.DeliveryDetails.Local.Select(x => x.Id == item.Id).Any())
			{
				throw new ApplicationException("Object already exists in context");
			}
			_context.Entry(item).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void Delete(DeliveryDetail item)
		{
			this._context.DeliveryDetails.Remove(item);
			_context.SaveChanges();
		}

		public DeliveryDetail Get(int id)
		{
			return _context.DeliveryDetails.FirstOrDefault(x => x.Id == id);
		}

		public int Count => _context.DeliveryDetails.Count();
	}
}
