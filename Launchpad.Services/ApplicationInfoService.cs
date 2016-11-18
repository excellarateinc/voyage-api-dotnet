using System.Linq;
using System.IO;
using Launchpad.Models;
using Launchpad.Core;
using Launchpad.Services.Interfaces;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Launchpad.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        private readonly IConfigurationManagerService _configurationManagerService;
        private readonly IFileReaderService _fileReaderService;
        private readonly IPathProviderService _pathProviderService;

        public ApplicationInfoService(IConfigurationManagerService configurationManagerService, IFileReaderService fileReaderService, IPathProviderService pathProviderService)
        {
            _configurationManagerService = configurationManagerService.ThrowIfNull(nameof(configurationManagerService));
            _fileReaderService = fileReaderService.ThrowIfNull(nameof(fileReaderService));
            _pathProviderService = pathProviderService.ThrowIfNull(nameof(pathProviderService));
        }

        public ApplicationInfoModel GetApplicationInfo()
        {
            var filePath = _pathProviderService.LocalPath;
            var fileName = _configurationManagerService.GetAppSetting("ApplicationInfoFileName");

            var text = _fileReaderService.ReadAllText(Path.Combine(filePath, fileName));

            var buildNumber = (string)JObject.Parse(text).SelectToken("build.buildNumber");

            return new ApplicationInfoModel()
            {
                BuildNumber = buildNumber
            };
        }
    }
}