using System.Web;
using Voyage.Core;
using Serilog;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Voyage.Web.Filters
{
    public class ClientAuthorizeAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext authenticationContext)
        {
            var urlInfo = authenticationContext.HttpContext.Request.QueryString["ReturnUrl"];
            if (string.IsNullOrWhiteSpace(urlInfo))
            {
                SetUnauthorizedResponse(authenticationContext);
                return;
            }

            var urlSegments = urlInfo.Split('?');
            if (urlSegments.Length < 2)
            {
                SetUnauthorizedResponse(authenticationContext);
                return;
            }

            var queries = HttpUtility.ParseQueryString(urlSegments[1]);
            var clientId = queries.Get("client_id");
            var redirectUri = queries.Get("redirect_uri");

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(redirectUri))
            {
                SetUnauthorizedResponse(authenticationContext);
                return;
            }

            // TODO Your application will need to validate client id and redirect url here
            if (Clients.Client1.Id == clientId)
            {
                if (Clients.Client1.RedirectUrl != redirectUri)
                    SetUnauthorizedResponse(authenticationContext);
            }
            else if (Clients.Client2.Id == clientId)
            {
                if (Clients.Client2.RedirectUrl != redirectUri)
                    SetUnauthorizedResponse(authenticationContext);
            }
            else
            {
                SetUnauthorizedResponse(authenticationContext);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // Impletation for this isn't require
        }

        private void SetUnauthorizedResponse(AuthenticationContext context)
        {
            // Check if the user has the correct claim
            Log.Logger
                .ForContext<ClaimAuthorizeAttribute>()
                .Information("({eventCode:l}) Unauthozied client.", EventCodes.Authorization);
            context.Result = new ContentResult
            {
                Content = "Unauthorized client."
            };
        }
    }
}
