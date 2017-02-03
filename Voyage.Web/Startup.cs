using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Voyage.Web.Startup))]

namespace Voyage.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configure(app);
        }
    }
}