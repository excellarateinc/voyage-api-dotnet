using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Owin;
using PhoneNumbers;
using Voyage.Models.Enum;
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
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.ValidatingUser };

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
                if (!string.IsNullOrEmpty(Request.Form.Get("submit.Submit")))
                {
                    var userName = Request.Form.Get("username");
                    var phonenumber = Request.Form.Get("phonenumber");

                    if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phonenumber))
                    {
                        model.HasError = true;
                        model.ErrorMessage = "User name and phone number are required fields.";
                    }
                    else
                    {
                        // validate phone number input
                        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                        var phoneNumber = phoneNumberUtil.Parse(phonenumber, "US");
                        var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                        if (isValid)
                        {
                            var context = HttpContext.GetOwinContext().GetAutofacLifetimeScope();
                            var userService = context.Resolve<IUserService>();

                            // validate user and phone number and send verification code
                            var user = await userService.GetUserByNameAsync(userName);
                            var userPhoneNumber = user.Phones.FirstOrDefault(c => c.PhoneNumber == phonenumber);
                            if (userPhoneNumber != null)
                            {
                                // user SNS to generate code -> save -> send
                                // 1. get SNS
                                // 2. get generated code
                                // 3. save generated code to database
                                // 4. send code via text message
                                // 5. save user name to session for next step
                                Session["userName"] = userName;
                            }
                        }

                        // set next step
                        model.ForgotPasswordStep = ForgotPasswordStep.VerifyingCode;
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form.Get("submit.VerifyCode")))
                {
                    // verify code
                    var code = Request.Form.Get("code");
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        model.HasError = true;
                        model.ErrorMessage = "Code to verify is a required field.";
                        model.ForgotPasswordStep = ForgotPasswordStep.VerifyingCode;
                    }
                    else
                    {
                        // 1. validate code here
                        // 2. get user questions by user name
                        var userName = Session["userName"];

                        // 3. add to model
                        // 4. set next step to verify answers
                        model.ForgotPasswordStep = ForgotPasswordStep.ValidatingQuestions;
                    }
                }
                else if (!string.IsNullOrEmpty(Request.Form.Get("submit.VerifyQuestions")))
                {
                    var answer1 = Request.Form.Get("answer1");
                    var answer2 = Request.Form.Get("answer2");

                    if (string.IsNullOrWhiteSpace(answer1) || string.IsNullOrWhiteSpace(answer2))
                    {
                        model.HasError = true;
                        model.ErrorMessage = "All security anwers are required field.";
                        model.ForgotPasswordStep = ForgotPasswordStep.ValidatingQuestions;
                    }
                    else
                    {
                        // 1. get answers by user name
                        var userName = Session["userName"];

                        // 2. verify answers
                        // 3. redirect to log in
                        return RedirectPermanent(Request.QueryString["ReturnUrl"]);
                    }
                }
            }
            catch (Exception ex)
            {
                // log error
            }

            return View(model);
        }
    }
}