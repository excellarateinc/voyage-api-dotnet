using System;
using System.Collections.Specialized;
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
using Voyage.Web.Models;
using System.DirectoryServices.AccountManagement;

namespace Voyage.Web.Controllers
{
    public class AccountController : Controller
    {
        public async Task<ActionResult> Login()
        {
            var loginModel = GetModel();
            if (Request.HttpMethod == "POST")
            {
                var isPersistent = !string.IsNullOrEmpty(Request.Form.Get("isPersistent"));

                if (!string.IsNullOrEmpty(Request.Form.Get("submit.Signin")))
                {
                    var context = HttpContext.GetOwinContext().GetAutofacLifetimeScope();
                    var userService = context.Resolve<IUserService>();
                    try
                    {
                        ClaimsIdentity identity = null;
                        var isValidCredential = await userService.IsValidCredential(Request.Form["username"], Request.Form["password"]);
                        if (!isValidCredential)
                        {

                            // In case the user enters the windows credentials. Not Found execption will be thrown if that also fails.
                            using (var adContext = new PrincipalContext(ContextType.Machine))
                            {
                               if (!adContext.ValidateCredentials(Request.Form["username"], Request.Form["password"]))
                                    throw new NotFoundException();
                               else
                                    identity = userService.CreateClientClaimsIdentityAsync(HttpUtility.ParseQueryString(Request.Params["ReturnUrl"])[0]);
                            }
                        }
                        else
                        {
                            identity = await userService.CreateClaimsIdentityAsync(Request.Form["username"], OAuthDefaults.AuthenticationType);
                        }

                        var authentication = HttpContext.GetOwinContext().Authentication;
                        authentication.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, new ClaimsIdentity(identity.Claims, "Application"));
                    }
                    catch (NotFoundException notFoundException)
                    {
                        loginModel.NotFoundException = notFoundException;
                        return View(loginModel);
                    }
                }
            }

            return View(loginModel);
        }

        public ActionResult Logout()
        {
            return View();
        }

        private LoginModel GetModel()
        {
            var loginModel = new LoginModel();
            try
            {
                // Get client return url
                loginModel.ReturnUrl = Server.UrlEncode(Request.QueryString["ReturnUrl"]);

                return loginModel;
            }
            catch (Exception)
            {
                return loginModel;
            }
        }
    }
}