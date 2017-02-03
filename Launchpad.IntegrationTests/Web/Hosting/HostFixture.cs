﻿using System;

using Voyage.IntegrationTests.Web.Client;
using Voyage.Web;

using Microsoft.Owin.Hosting;

namespace Voyage.IntegrationTests.Web.Hosting
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
