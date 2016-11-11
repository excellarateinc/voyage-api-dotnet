using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Models.Enum;
using Launchpad.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API.V1
{
   
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    [AllowAnonymous]
    public class ApplicationInfoController : ApiController
    {
        private readonly IApplicationInfoService _applicationInfoService;
        private readonly IConfigurationManagerService _configurationManagerService;
        private readonly ITupleFileReaderService _tupleFileReaderService;
        private readonly Func<string> _localPathProvider;

        public ApplicationInfoController(IApplicationInfoService applicationInfoService, IConfigurationManagerService configurationManagerService, ITupleFileReaderService tupleFileReaderService, Func<string> localPathProvider)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
            _configurationManagerService = configurationManagerService.ThrowIfNull(null);
            _tupleFileReaderService = tupleFileReaderService.ThrowIfNull(null);
            _localPathProvider = localPathProvider;
        }

        /**
        * @api {get} /v1/status Get application info
        * @apiVersion 0.1.0
        * @apiName GetStatus
        * @apiGroup Status
        *   
        * @apiSuccess {String} version Version Number
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {   
        *           "buildNumber": "some_number"
        *       }
        *   ]
        **/
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var settings = new Dictionary<string, string>();

            var filePath = _localPathProvider();
            var fileName = _configurationManagerService.GetAppSetting("ApplicationInfoFileName");

            var text = _tupleFileReaderService.ReadAllText(Path.Combine(filePath, fileName));

            var buildNumber = (string)JObject.Parse(text).SelectToken("build.buildNumber");

            settings.Add("buildNumber", buildNumber);

            var appInfo = _applicationInfoService.GetApplicationInfo(settings);
            
            return Ok(appInfo);
        }
    }
}