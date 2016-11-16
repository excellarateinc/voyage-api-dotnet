using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
{
    public interface IConfigurationManagerService
    {
        string GetAppSetting(string key);
    }
}
