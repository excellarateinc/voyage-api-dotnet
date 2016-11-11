using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Models.Enum;
using Launchpad.Services.Interfaces;
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

        public ApplicationInfoController(IApplicationInfoService applicationInfoService, IConfigurationManagerService configurationManagerService, ITupleFileReaderService tupleFileReaderService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
            _configurationManagerService = configurationManagerService.ThrowIfNull(null);
            _tupleFileReaderService = tupleFileReaderService.ThrowIfNull(null);
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
        *           "version": "version"
        *       }
        *   ]
        **/
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var settings = new Dictionary<string, string>();

            var filePath = _configurationManagerService.GetAppSetting("ApplicationInfoFilePath");
            var fileName = _configurationManagerService.GetAppSetting("ApplicationInfoFileName");

            var lines = _tupleFileReaderService.ReadAllLines(Path.Combine(filePath, fileName));

            lines.ToList().ForEach(l =>
            {
                var setting = l.Split('=');

                settings.Add(setting[0], setting[1]);
            });

            var appInfo = _applicationInfoService.GetApplicationInfo(settings);
            
            return Ok(appInfo);
        }
    }
}