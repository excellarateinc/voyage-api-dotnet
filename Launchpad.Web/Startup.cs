using Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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