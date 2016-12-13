using Launchpad.Web.IntegrationTests.Client;
using Microsoft.Owin.Hosting;
using System;

namespace Launchpad.Web.IntegrationTests.Hosting
{
    /// <summary>
    /// Test fixture responsible for bootstrapping and opening the self hosted services 
    /// </summary>
    public class HostFixture : IDisposable
    {
        private readonly IDisposable _webAppInstance;

        public HostFixture()
        {
            _webAppInstance = WebApp.Start<Startup>(url: HostingOptions.BaseAddress);

            TokenProvider.Instance.Configure().Wait();
        }

        public void Dispose()
        {
            if (_webAppInstance != null)
            {
                _webAppInstance.Dispose();
                TokenProvider.Instance.Dispose();
            }
        }
    }
}
