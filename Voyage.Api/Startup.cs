using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Voyage.Api.Startup))]
namespace Voyage.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configure(app);
        }
    }
}