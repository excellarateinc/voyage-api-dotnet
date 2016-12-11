using Launchpad.Services.Interfaces;
using System.Configuration;

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
