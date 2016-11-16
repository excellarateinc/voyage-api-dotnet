using Launchpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Launchpad.Services
{
    public class ConfigurationManagerService : IConfigurationManagerService
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
