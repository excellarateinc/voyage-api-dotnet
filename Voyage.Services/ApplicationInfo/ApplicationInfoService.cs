using System;
using System.IO;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.FileReader;
using Newtonsoft.Json.Linq;

namespace Voyage.Services.ApplicationInfo
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly string _fileName;

        public ApplicationInfoService(IFileReaderService fileReaderService, string fileName)
        {
            _fileReaderService = fileReaderService.ThrowIfNull(nameof(fileReaderService));
            _fileName = fileName.ThrowIfNullOrEmpty(nameof(fileName));
        }

        public ApplicationInfoModel GetApplicationInfo()
        {
            var text = _fileReaderService.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName));

            var buildNumber = (string)JObject.Parse(text).SelectToken("build.buildNumber");

            return new ApplicationInfoModel
            {
                BuildNumber = buildNumber
            };
        }
    }
}