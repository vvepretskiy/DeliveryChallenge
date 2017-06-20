using DeliveryChallenge.Logger;

namespace DeliveryChallenge.Models
{
	public class DataConfiguration : System.Data.Entity.DbConfiguration
	{
		public DataConfiguration()
		{
			AddInterceptor(new LoggingCommandInterceptor());
		}
	}
}
