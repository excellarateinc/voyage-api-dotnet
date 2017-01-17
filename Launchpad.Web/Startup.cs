using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Launchpad.Web.Startup))]

namespace Launchpad.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configure(app);
        }
    }
}