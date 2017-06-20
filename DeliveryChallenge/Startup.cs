using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeliveryChallenge.Startup))]
namespace DeliveryChallenge
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
