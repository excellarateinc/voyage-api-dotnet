using System;
using Launchpad.IntegrationTests.Web.Client;

namespace Launchpad.IntegrationTests.Web.Hosting
{
    /// <summary>
    /// Test fixture responsible for bootstrapping and opening the self hosted services
    /// </summary>
    public class HostFixture : IDisposable
    {
        public HostFixture()
        {
            TokenProvider.Instance.Configure().Wait();
        }

        public void Dispose()
        {
            TokenProvider.Instance.Dispose();
        }
    }
}
