using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Models;
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
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.VerifyUser };

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "User name and phone number are required fields.");
            }

            // 1. get records from audit to check if password attapmt within the last 20 minutes is more than 5 times
            var attempts = _auditService.GetAuditActivityWithinTime(userName, "/passwordRecovery", 20);

            // 3. insert audit log
            await WriteAuditLog(userName);

            // 2. there are 5 or more attempts so we won't send verification code
            if (attempts.Count >= 5)
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "Too many attempt to recover password. Please wait for 20 minutes.");
            }

            // 4. validate phone number input
            var formatedPhoneNumber = string.Empty;
            if (_phoneService.IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber))
            {
                // validate user and phone number and send verification code
                var user = await _userService.GetUserByNameAsync(userName);
                var userPhoneNumber = user.Phones.FirstOrDefault(c => c.PhoneNumber == formatedPhoneNumber);
                if (userPhoneNumber != null)
                {
                    // generate password recovery token
                    var passwordRecoverToken = await _userService.GeneratePasswordResetTokenAsync(user.Id);
                    user.PasswordRecoveryToken = passwordRecoverToken;
                    await _userService.UpdateUserAsync(user.Id, user);

                    // generate security code
                    var securityCode = _phoneService.GenerateSecurityCode();

                    // save to our database
                    _phoneService.InsertSecurityCode(userPhoneNumber.Id, securityCode);

                    // send text to user using amazon SNS
                    await _phoneService.SendSecurityCodeAsync(formatedPhoneNumber, securityCode);

                    // set user id and password recover token in model for session use
                    model.UserId = user.Id;
                }
            }

            // 5. set next step
            model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode;

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
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode };

            // check for empty security code
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "Security code is a required field.");
            }

            if (await _phoneService.IsValidSecurityCodeAsync(appUser.UserId, code))
            {
                await _phoneService.ClearUserPhoneSecurityCodeAsync(appUser.UserId);

                // set next step to verify answers
                // TODO bypass security questions/answers for now
                // model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityAnswers;
                model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;
            }
            else
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "Provided code is invalid.");
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
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.VerifySecurityAnswers };

            if (anwers.Any(c => c == string.Empty))
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "All answers are required.");
            }

            // TODO validating answers will go here
            model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;

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
            var model = new ForgotPasswordModel { ForgotPasswordStep = ForgotPasswordStep.ResetPassword };
            if (newPassword != confirmNewPassword)
            {
                throw new PasswordRecoverException(model.ForgotPasswordStep, "New password must match with New confirm password.");
            }

            var user = await _userService.GetUserAsync(appUser.UserId);
            var identity = await _userService.ResetPassword(user.Id, user.PasswordRecoveryToken, newPassword);
            if (!identity.Errors.Any())
            {
                user.PasswordRecoveryToken = string.Empty;
                await _userService.UpdateUserAsync(user.Id, user);

                return model;
            }

            ThrowPasswordRecoverException(identity.Errors, ForgotPasswordStep.ResetPassword);

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
        /// throw password recovery exception
        /// </summary>
        /// <param name="errorMessages"></param>
        /// <param name="step"></param>
        private void ThrowPasswordRecoverException(IEnumerable<string> errorMessages, ForgotPasswordStep step)
        {
            var builder = new StringBuilder();
            foreach (var identityError in errorMessages)
            {
                builder.Append(identityError);
                builder.AppendLine();
            }

            throw new PasswordRecoverException(step, builder.ToString());
        }
    }
}
