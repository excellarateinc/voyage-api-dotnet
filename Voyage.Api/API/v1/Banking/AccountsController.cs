using System.IdentityModel.Claims;
using Voyage.Core;
using System.Web.Http;
using Microsoft.Owin.Security;
using Voyage.Api.Filters;
using Voyage.Models;
using Voyage.Services.Banking;

namespace Voyage.Api.API.V1.Banking
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]    
    public class AccountsController : ApiController
    {
        private readonly IAccountsService _accountsService;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountsController(IAccountsService accountsService, IAuthenticationManager authenticationManager)
        {
            _accountsService = accountsService.ThrowIfNull(nameof(accountsService));
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        [HttpGet]
        [Route("banking/accounts")]
        [ClaimAuthorize(ClaimValue = AppClaims.GetAccounts)]
        public IHttpActionResult GetAccounts()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var accounts = _accountsService.GetAccounts(userId);
            return Ok(accounts);
        }

        [HttpGet]
        [Route("banking/accounts/transactions")]
        public IHttpActionResult GetTransactionHistory()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var transactionHistory = _accountsService.GetTransactionHistory(userId);
            return Ok(transactionHistory);
        }

        [HttpPost]
        [Route("banking/accounts/transfers")]
        public IHttpActionResult Transfer(TransferModel transfer)
        {
            _accountsService.Transfer(transfer);
            return Ok();
        }
    }
}