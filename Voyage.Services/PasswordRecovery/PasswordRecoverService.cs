using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Models;
using Voyage.Models.Enum;
using Voyage.Services.Audit;
using Voyage.Services.Phone;
using Voyage.Services.User;
using Voyage.Web.Models;

namespace Voyage.Services.PasswordRecovery
{
    public class PasswordRecoverService : IPasswordRecoverService
    {
        private readonly IPhoneService _phoneService;
        private readonly IUserService _userService;
        private readonly IAuditService _auditService;

        public PasswordRecoverService(IPhoneService phoneService, IUserService userService, IAuditService auditService)
        {
            _phoneService = phoneService;
            _userService = userService;
            _auditService = auditService;
        }

        /// <summary>
        /// Validate user name and phone number before sending security code
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<ForgotPasswordModel> ValidateUserInfoAsync(string userName, string phoneNumber)
        {
            var model = new ForgotPasswordModel();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                SetModelError(model, "User name and phone number are required fields.");
            }
            else
            {
                // validate phone number input
                var formatedPhoneNumber = string.Empty;
                if (_phoneService.IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber))
                {
                    // validate user and phone number and send verification code
                    var user = await _userService.GetUserByNameAsync(userName);
                    var userPhoneNumber = user.Phones.FirstOrDefault(c => c.PhoneNumber == formatedPhoneNumber);
                    if (userPhoneNumber != null)
                    {
                        // 1. get records from audit to check if password attapmt within the last 20 minutes is more than 5 times
                        var attempts = _auditService.GetAuditActivityWithinTime(userName, "/passwordRecovery", 20);

                        // there are 5 or more attempts so we won't send verification code
                        if (attempts.Count >= 5)
                        {
                            SetModelError(model, "Too many attempt to recover password. Please wait for 20 minutes.");

                            // write audit log using as starting 20 minutes wait
                            await WriteAuditLog(userName);

                            return model;
                        }

                        // insert audit log
                        await WriteAuditLog(userName);

                        // generate password recovery token
                        var passwordRecoverToken = await _userService.GeneratePasswordResetTokenAsync(user.Id);
                        user.PasswordRecoveryToken = passwordRecoverToken;
                        await _userService.UpdateUserAsync(user.Id, user);

                        // generate security code
                        var securityCode = _phoneService.GenerateSecurityCode();

                        // save to our database
                        _phoneService.InsertSecurityCode(userPhoneNumber.Id, securityCode);

                        // send text to user using amazon SNS
                        await _phoneService.SendSecurityCode(formatedPhoneNumber, securityCode);

                        // set user id and password recover token in model for session use
                        model.UserId = user.Id;
                    }
                }

                // set next step
                model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode;
            }

            return model;
        }

        /// <summary>
        /// validate security code before moving to next step
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ForgotPasswordModel> VerifyCodeAsync(UserApplicationSession appUser, string code)
        {
            var model = new ForgotPasswordModel();

            // check for empty security code
            if (string.IsNullOrWhiteSpace(code))
            {
                SetModelError(model, "Security code is a required field.", ForgotPasswordStep.VerifySecurityCode);
            }
            else
            {
                // verify security code
                var user = await _userService.GetUserAsync(appUser.UserId);
                var phone = user.Phones.FirstOrDefault(c => c.VerificationCode == code);

                // check if security code found
                if (phone == null)
                {
                    SetModelError(model, "Provided code is invalid.", ForgotPasswordStep.VerifySecurityCode);
                }
                else
                {
                    // reset phone security code after it was used
                    _phoneService.ResetSecurityCode(phone.Id);

                    // set next step to verify answers
                    // TODO bypass security questions/answers for now
                    // model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityAnswers;
                    model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;
                }
            }

            return model;
        }

        /// <summary>
        /// validate user security ansers
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="anwers"></param>
        /// <returns></returns>
        public ForgotPasswordModel VerifySecurityAnswers(string userId, List<string> anwers)
        {
            var model = new ForgotPasswordModel();

            if (anwers.Any(c => c == string.Empty))
            {
                SetModelError(model, "All answers are required.", ForgotPasswordStep.VerifySecurityAnswers);
            }
            else
            {
                model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;
            }

            return model;
        }

        /// <summary>
        /// reseting password step
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmNewPassword"></param>
        /// <returns></returns>
        public async Task<ForgotPasswordModel> ResetPasswordAsync(UserApplicationSession appUser, string newPassword, string confirmNewPassword)
        {
            var model = new ForgotPasswordModel();
            if (newPassword != confirmNewPassword)
            {
                SetModelError(model, "New password must match with New confirm password.", ForgotPasswordStep.ResetPassword);
            }
            else
            {
                var user = await _userService.GetUserAsync(appUser.UserId);
                var identity = await _userService.ChangePassword(user.Id, user.PasswordRecoveryToken, newPassword);
                if (!identity.Errors.Any())
                    return model;

                SetModelError(model, identity.Errors, ForgotPasswordStep.ResetPassword);
            }

            return model;
        }

        /// <summary>
        /// create audit log
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task WriteAuditLog(string userName)
        {
            await _auditService.RecordAsync(new ActivityAuditModel
            {
                UserName = userName,
                Date = DateTime.UtcNow,
                StatusCode = 200,
                Path = "/passwordRecovery",
                RequestId = Guid.NewGuid().ToString(),
                Error = string.Empty,
                IpAddress = string.Empty,
                Method = string.Empty
            });
        }

        /// <summary>
        /// set error to model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMessages"></param>
        /// <param name="step"></param>
        private void SetModelError(ForgotPasswordModel model, IEnumerable<string> errorMessages, ForgotPasswordStep step)
        {
            var builder = new StringBuilder();
            foreach (var identityError in errorMessages)
            {
                builder.Append(identityError);
                builder.AppendLine();
            }

            SetModelError(model, builder.ToString(), step);
        }

        /// <summary>
        /// set error to model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMessage"></param>
        /// <param name="step"></param>
        private void SetModelError(ForgotPasswordModel model, string errorMessage, ForgotPasswordStep step = ForgotPasswordStep.VerifyUser)
        {
            model.HasError = true;
            model.ErrorMessage = errorMessage;
            model.ForgotPasswordStep = step;
        }
    }
}
