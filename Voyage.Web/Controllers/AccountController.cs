using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Voyage.Core.Exceptions;
using Voyage.Services.User;

namespace Voyage.Web.Controllers
{
    public class AccountController : Controller
    {
        public async Task<ActionResult> Login()
        {
            var authentication = HttpContext.GetOwinContext().Authentication;
            if (Request.HttpMethod == "POST")
            {
                var isPersistent = !string.IsNullOrEmpty(Request.Form.Get("isPersistent"));

                if (!string.IsNullOrEmpty(Request.Form.Get("submit.Signin")))
                {
                    var context = HttpContext.GetOwinContext().GetAutofacLifetimeScope();
                    var userService = context.Resolve<IUserService>();
                    try
                    {
                        var isValidCredential = await userService.IsValidCredential(Request.Form["username"], Request.Form["password"]);
                        if (!isValidCredential)
                            throw new NotFoundException();

                        ClaimsIdentity identity = await userService.CreateClaimsIdentityAsync(Request.Form["username"], OAuthDefaults.AuthenticationType);
                        authentication.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, new ClaimsIdentity(identity.Claims, "Application"));
                    }
                    catch (NotFoundException notFoundException)
                    {
                        return View(notFoundException);
                    }
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}