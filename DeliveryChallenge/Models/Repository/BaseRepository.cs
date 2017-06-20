namespace DeliveryChallenge.Models.Repository
{
	public class BaseRepository : ModelDbContext
	{
		protected readonly ModelDbContext _context;

		public BaseRepository(IModelDbContext context)
		{
			this._context = context as ModelDbContext;
		}
	}
}
