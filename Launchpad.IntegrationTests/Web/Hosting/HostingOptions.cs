using System.Configuration;

namespace Launchpad.IntegrationTests.Web.Hosting
{
    public static class HostingOptions
    {
        public static readonly string BaseAddress = ConfigurationManager.AppSettings["BaseAddress"];
    }
}
