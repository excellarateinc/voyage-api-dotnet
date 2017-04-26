using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Owin;
using Voyage.Models;
using Voyage.Models.Enum;
using Voyage.Services.Audit;
using Voyage.Services.PasswordRecovery;
using Voyage.Services.Phone;
using Voyage.Services.User;
using Voyage.Web.Filters;
using Voyage.Web.Models;

namespace Voyage.Web.Controllers
{
    public class ForgotpasswordController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.VerifyUser };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClientAuthorize]
        public async Task<ActionResult> Index(ForgotPasswordModel model)
        {
            if (Request.HttpMethod != "POST")
                return View(model);

            try
            {
                var context = HttpContext.GetOwinContext().GetAutofacLifetimeScope();
                var passwordRecoverService = context.Resolve<IPasswordRecoverService>();

                if (!string.IsNullOrEmpty(Request.Form.Get("submit.VerifyUser")))
                {
                    model = await passwordRecoverService.ValidateUserInfoAsync(Request.Form.Get("username"), Request.Form.Get("phonenumber"));

                    if (!model.HasError)
                    {
                        // 5. save user name to session for next step
                        Session["appUser"] = new UserApplicationSession
                        {
                            UserId = model.UserId
                        };
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form.Get("submit.VerifySecurityCode")))
                {
                    // verify code
                    var appUser = Session["appUser"] as UserApplicationSession;
                    model = await passwordRecoverService.VerifyCodeAsync(appUser?.UserId, Request.Form.Get("code"));

                    // after verifying code successfully save code for later use
                    if (!model.HasError)
                    {
                        appUser.PasswordRecoveryToken = model.PasswordRecoveryToken;
                        Session["appUser"] = appUser;
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form.Get("submit.VerifySecurityAnswers")))
                {
                    var appUser = Session["appUser"] as UserApplicationSession;
                    model = passwordRecoverService.VerifySecurityAnswers(appUser?.UserId, new List<string>());
                }
                else if (!string.IsNullOrEmpty(Request.Form.Get("submit.ResetPassword")))
                {
                    var appUser = Session["appUser"] as UserApplicationSession;
                    model = await passwordRecoverService.ResetPasswordAsync(Session["userName"].ToString(), appUser?.PasswordRecoveryToken, Request.Form.Get("newpassword"), Request.Form.Get("confirmnewpassword"));

                    // redirect user to log in if no error
                    if (!model.HasError)
                    {
                        Session.Clear();
                        return RedirectPermanent(Request.QueryString["ReturnUrl"]);
                    }
                }
            }
            catch (Exception ex)
            {
                // log error to database
            }

            return View(model);
        }
    }
}