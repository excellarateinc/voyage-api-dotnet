using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Voyage.Api.Startup))]
namespace Voyage.Api
{
    /// <summary>
    /// Application Startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the middleware.
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            Configure(app);
        }
    }
}