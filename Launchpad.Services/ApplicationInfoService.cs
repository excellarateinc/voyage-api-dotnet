using System.Linq;
using System.IO;
using Launchpad.Models;
using Launchpad.Core;
using Launchpad.Services.Interfaces;
using System.Collections.Generic;

namespace Launchpad.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public ApplicationInfoModel GetApplicationInfo(Dictionary<string, string> settings)
        {
            return new ApplicationInfoModel()
            {
                BuildNumber = settings["buildNumber"]
            };
        }
    }
}