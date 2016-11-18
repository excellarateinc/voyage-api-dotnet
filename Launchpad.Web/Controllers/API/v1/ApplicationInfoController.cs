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
        private readonly IFileReaderService _fileReaderService;
        private readonly IPathProviderService _pathProviderService;

        public ApplicationInfoController(IApplicationInfoService applicationInfoService, IConfigurationManagerService configurationManagerService, IFileReaderService fileReaderService, IPathProviderService pathProviderService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
            _configurationManagerService = configurationManagerService.ThrowIfNull(null);
            _fileReaderService = fileReaderService.ThrowIfNull(null);
            _pathProviderService = pathProviderService;
        }

        /**
        * @api {get} /v1/statuses Get application info
        * @apiVersion 0.1.0
        * @apiName GetStatuses
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
            var appInfo = _applicationInfoService.GetApplicationInfo();
            
            return Ok(appInfo);
        }
    }
}