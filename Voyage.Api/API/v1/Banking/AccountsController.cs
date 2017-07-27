using Voyage.Core;
using System.Web.Http;
using Voyage.Services.ApplicationInfo;

namespace Voyage.Api.API.V1.Banking
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    [AllowAnonymous]
    public class AccountsController : ApiController
    {
        private readonly IApplicationInfoService _applicationInfoService;

        public AccountsController(IApplicationInfoService applicationInfoService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
        }

        [HttpGet]
        [Route("banking/accounts")]
        public IHttpActionResult GetAccounts()
        {
            return Ok("GetAccounts() works!");
        }

        [HttpGet]
        [Route("banking/accounts/transactions")]
        public IHttpActionResult GetTransactionHistory()
        {
            var appInfo = _applicationInfoService.GetApplicationInfo();
            return Ok(appInfo);
        }

        [HttpPost]
        [Route("banking/accounts/transfers")]
        public IHttpActionResult Transfer()
        {
            var appInfo = _applicationInfoService.GetApplicationInfo();
            return Ok(appInfo);
        }
    }
}