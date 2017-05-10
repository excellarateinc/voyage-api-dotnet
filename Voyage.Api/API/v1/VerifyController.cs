using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Services.Phone;

namespace Voyage.Api.API.v1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class VerifyController : ApiController
    {
        private readonly IPhoneService _phoneService;

        public VerifyController(IPhoneService phoneService)
        {
            _phoneService = phoneService.ThrowIfNull(nameof(phoneService));
        }

        /**
        * @api {get} /v1/Verify/send Send verify code
        * @apiVersion 0.1.0
        * @apiName PostVerifySend
        * @apiGroup Verify
        *
        * @apiPermission /**
        * @api {get} /v1/users Get all users
        * @apiVersion 0.1.0
        * @apiName GetUsers
        * @apiGroup User
        *
        * @apiPermission authorized
        *
        * @apiUse AuthHeader
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 NO CONTENT
        *
        * @apiUse UnauthorizedError
        **/
        [Authorize]
        [HttpGet]
        [Route("verify/send")]
        public async Task<IHttpActionResult> SendVerificationCode()
        {
            await _phoneService.SendSecurityCodeToUserPhoneNumber(User.Identity.Name); 
            return Ok();
        }
    }
}