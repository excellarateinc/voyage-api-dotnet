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

        public async Task<ForgotPasswordModel> ValidateUserInfoAsync(string userName, string phoneNumber)
        {
            var model = new ForgotPasswordModel();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                model.HasError = true;
                model.ErrorMessage = "User name and phone number are required fields.";
            }
            else
            {
                // validate phone number input
                var formatedPhoneNumber = string.Empty;
                var isValid = _phoneService.IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber);
                if (isValid)
                {
                    // validate user and phone number and send verification code
                    var user = await _userService.GetUserByNameAsync(userName);
                    var userPhoneNumber = user.Phones.FirstOrDefault(c => c.PhoneNumber == phoneNumber);
                    if (userPhoneNumber != null)
                    {
                        // 1. get records from audit to check if password attapmt within 20 minutes is more than 5 times
                        // 1-1. if so we will show message to user to wait for 20 minutes
                        var attempts = _auditService.GeAudittActivityWithinTime(userName, "/passwordRecovery", 20);

                        // 5 or more attempts so we won't send verification code
                        if (attempts.Count() >= 5)
                        {
                            return model;
                        }

                        // 2. insert audit log
                        await _auditService.RecordAsync(new ActivityAuditModel
                        {
                            UserName = userName,
                            Date = DateTime.UtcNow,
                            StatusCode = 200,
                            Path = "/passwordRecovery",
                            RequestId = Guid.Empty.ToString(),
                            Error = string.Empty,
                            IpAddress = string.Empty,
                            Method = string.Empty
                        });

                        // generate password recovery token
                        var passwordRecoverToken = await _userService.GeneratePasswordResetTokenAsync(user.Id);
                        user.PasswordRecoveryToken = passwordRecoverToken;
                        await _userService.UpdateUserAsync(user.Id, user);

                        // generate verification code
                        var verificationCode = _phoneService.GenerateVerificationCode();

                        // save to our database
                        _phoneService.InsertVerificationCode(userPhoneNumber.Id, verificationCode);

                        // send text to user using amazon SNS

                        // set user id in model for session use
                        model.UserId = user.Id;
                    }
                }

                // set next step
                model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode;
            }

            return model;
        }

        public async Task<ForgotPasswordModel> VerifyCodeAsync(string userId, string code)
        {
            var model = new ForgotPasswordModel();

            // verify code
            if (string.IsNullOrWhiteSpace(code))
            {
                model.HasError = true;
                model.ErrorMessage = "Code to verify is a required field.";
                model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode;
            }
            else
            {
                // verify security code
                var user = await _userService.GetUserAsync(userId);
                var phone = user.Phones.FirstOrDefault(c => c.VerificationCode == code);
                if (phone == null)
                {
                    model.HasError = true;
                    model.ErrorMessage = "Provided code is invalid.";
                    model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityCode;
                }
                else
                {
                    // reset phone security code after it was used
                    _phoneService.ResetVerificationCode(phone.Id);

                    // get password recovery token for user
                    model.PasswordRecoveryToken = user.PasswordRecoveryToken;

                    // set next step to verify answers
                    // TODO bypass security questions/answers for now
                    // model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityAnswers;
                    model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;
                }
            }

            return model;
        }

        public ForgotPasswordModel VerifySecurityAnswers(string userId, List<string> anwers)
        {
            var model = new ForgotPasswordModel();

            if (anwers.Any(c => c == string.Empty))
            {
                model.HasError = true;
                model.ErrorMessage = "All answers are required.";
                model.ForgotPasswordStep = ForgotPasswordStep.VerifySecurityAnswers;
            }
            else
            {
                model.ForgotPasswordStep = ForgotPasswordStep.ResetPassword;
            }

            return model;
        }

        public async Task<ForgotPasswordModel> ResetPasswordAsync(string userName, string passwordRecoveryToken, string newPassword, string confirmNewPassword)
        {
            var model = new ForgotPasswordModel();
            if (newPassword != confirmNewPassword)
            {
                model.HasError = true;
                model.ErrorMessage = "New password must match with New confirm password.";
            }
            else
            {
                var identity = await _userService.ChangePassword(userName, passwordRecoveryToken, newPassword);
                if (!identity.Errors.Any())
                    return model;

                model.HasError = true;
                model.ErrorMessage = "Error while changing your password.";
                model.ForgotPasswordStep = ForgotPasswordStep.VerifyUser;
            }

            return model;
        }
    }
}
