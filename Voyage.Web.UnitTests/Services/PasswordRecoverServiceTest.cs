using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Models.Enum;
using Voyage.Services.Audit;
using Voyage.Services.PasswordRecovery;
using Voyage.Services.Phone;
using Voyage.Services.User;
using Voyage.Web.Models;
using Voyage.Web.UnitTests.Common.AutoMapperFixture;
using Xunit;

namespace Voyage.Web.UnitTests.Services
{
    [Trait("Category", "ForgotPassword.Service")]
    [Collection(AutoMapperCollection.CollectionName)]
    public class PasswordRecoverServiceTest
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IPhoneService> _phoneServiceMock;
        private Mock<IAuditService> _auditServiceMock;

        [Fact]
        public async Task ValidateUserInfo_must_check_for_empty_inputs()
        {
            CreateNewMockServices();
            var model = await GetPasswordRecoverService().ValidateUserInfoAsync(string.Empty, string.Empty);

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
        }

        [Fact]
        public async Task ValidateUerInfo_must_stop_the_attempt_when_more_than_5_times_already()
        {
            CreateNewMockServices();
            var phoneNumber = string.Empty;
            var audits = new List<ActivityAudit> { new ActivityAudit(), new ActivityAudit(), new ActivityAudit(), new ActivityAudit(), new ActivityAudit(), new ActivityAudit() };
            _auditServiceMock.Setup(c => c.GetAuditActivityWithinTime("TestUserName", "/passwordRecovery", 20)).Returns(audits);
            _auditServiceMock.Setup(c => c.RecordAsync(It.IsAny<ActivityAuditModel>())).Returns(Task.Delay(0));
            _phoneServiceMock.Setup(c => c.IsValidPhoneNumber(It.IsAny<string>(), out phoneNumber)).Returns(false);

            var model = await GetPasswordRecoverService().ValidateUserInfoAsync("TestUserName", "InvalidPhone");

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
        }

        [Fact]
        public async Task ValidateUserInfo_must_validate_user_phone_that_sent_in()
        {
            CreateNewMockServices();
            var phoneNumber = string.Empty;
            _auditServiceMock.Setup(c => c.GetAuditActivityWithinTime("TestUserName", "/passwordRecovery", 20)).Returns(new List<ActivityAudit>());
            _auditServiceMock.Setup(c => c.RecordAsync(It.IsAny<ActivityAuditModel>())).Returns(Task.Delay(0));
            _phoneServiceMock.Setup(c => c.IsValidPhoneNumber("InvalidPhone", out phoneNumber)).Returns(false);

            var model = await GetPasswordRecoverService().ValidateUserInfoAsync("TestUserName", "InvalidPhone");

            _phoneServiceMock.Verify();
            Assert.Equal(model.ForgotPasswordStep, ForgotPasswordStep.VerifySecurityCode);
        }

        [Fact]
        public async Task If_user_is_valid_but_phone_is_not_valid_token_must_not_be_generate()
        {
            CreateNewMockServices();
            var phones = new List<UserPhoneModel>
            {
                new UserPhoneModel
                {
                    PhoneNumber = "TestPhoneNumber"
                }
            };
            var user = new UserModel
            {
                Phones = phones
            };
            var phoneNumber = string.Empty;
            _auditServiceMock.Setup(c => c.GetAuditActivityWithinTime(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new List<ActivityAudit>());
            _auditServiceMock.Setup(c => c.RecordAsync(It.IsAny<ActivityAuditModel>())).Returns(Task.Delay(0));
            _phoneServiceMock.Setup(c => c.IsValidPhoneNumber(It.IsAny<string>(), out phoneNumber)).Returns(true);
            _userServiceMock.Setup(c => c.GetUserByNameAsync("TestUserName")).ReturnsAsync(user);
            _userServiceMock.Setup(c => c.GeneratePasswordResetTokenAsync(It.IsAny<string>())).ReturnsAsync("MockData");

            await GetPasswordRecoverService().ValidateUserInfoAsync("TestUserName", "NotValid");

            _userServiceMock.Verify(c => c.GeneratePasswordResetTokenAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task If_phone_is_valid_and_user_is_valid_token_must_be_generated()
        {
            CreateNewMockServices();
            var userId = "UserId";
            var securityCode = "SecurityCode";
            var userName = "TestUserName";
            var phoneNum = "TestPhoneNumber";
            var phoneNumberOut = string.Empty;
            var phones = new List<UserPhoneModel>
            {
                new UserPhoneModel
                {
                    PhoneNumber = string.Empty
                }
            };
            var user = new UserModel
            {
                Phones = phones,
                Id = userId
            };
            _auditServiceMock.Setup(c => c.GetAuditActivityWithinTime(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new List<ActivityAudit>());
            _auditServiceMock.Setup(c => c.RecordAsync(It.IsAny<ActivityAuditModel>())).Returns(Task.Delay(0));
            _phoneServiceMock.Setup(c => c.IsValidPhoneNumber(It.IsAny<string>(), out phoneNumberOut)).Returns(true);
            _userServiceMock.Setup(c => c.GetUserByNameAsync(userName)).ReturnsAsync(user);
            _userServiceMock.Setup(c => c.GeneratePasswordResetTokenAsync(It.IsAny<string>())).ReturnsAsync("PasswordToken");
            _userServiceMock.Setup(c => c.UpdateUserAsync(userId, It.IsAny<UserModel>())).ReturnsAsync(new UserModel());
            _phoneServiceMock.Setup(c => c.GenerateSecurityCode()).Returns(securityCode);
            _phoneServiceMock.Setup(c => c.InsertSecurityCode(It.IsAny<int>(), securityCode));
            _phoneServiceMock.Setup(c => c.SendSecurityCode(It.IsAny<string>(), securityCode)).Returns(Task.Delay(0));

            var model = await GetPasswordRecoverService().ValidateUserInfoAsync(userName, phoneNum);

            _userServiceMock.Verify();
            _phoneServiceMock.Verify(c => c.GenerateSecurityCode(), Times.Once);
            _phoneServiceMock.Verify();
            Assert.Equal(model.UserId, userId);
        }

        [Fact]
        public async Task ValifyCode_must_check_for_empty_code()
        {
            CreateNewMockServices();
            var model = await GetPasswordRecoverService().VerifyCodeAsync(new UserApplicationSession(), string.Empty);

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
        }

        [Fact]
        public async Task ValifyCode_must_valify_invalid_code_and_add_error_once_empty_check_is_passed()
        {
            CreateNewMockServices();
            var userId = "UserId";
            var securityCode = "SecurityCode";
            var appUser = new UserApplicationSession
            {
                UserId = userId,
                PasswordRecoveryToken = "RecoveryToken"
            };
            _phoneServiceMock.Setup(c => c.IsValidSecurityCode(userId, securityCode)).ReturnsAsync(false);

            var model = await GetPasswordRecoverService().VerifyCodeAsync(appUser, "NotValidCode");

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
            Assert.Equal(model.ForgotPasswordStep, ForgotPasswordStep.VerifySecurityCode);
        }

        [Fact]
        public async Task ValifyCode_must__move_to_next_step__checks_are_passed()
        {
            CreateNewMockServices();
            var userId = "UserId";
            var securityCode = "SecurityCode";
            var appUser = new UserApplicationSession
            {
                UserId = userId,
                PasswordRecoveryToken = "RecoveryToken"
            };
            _phoneServiceMock.Setup(c => c.IsValidSecurityCode(userId, securityCode)).ReturnsAsync(true);
            _phoneServiceMock.Setup(c => c.ClearUserPhoneSecurityCode(It.IsAny<string>())).Returns(Task.Delay(0));

            var model = await GetPasswordRecoverService().VerifyCodeAsync(appUser, securityCode);

            Assert.False(model.HasError);
            Assert.Null(model.ErrorMessage);
            Assert.Equal(model.ForgotPasswordStep, ForgotPasswordStep.ResetPassword);
        }

        public async Task ResetPassword_must_validate_matching_password_with_confirm_password()
        {
            CreateNewMockServices();
            var model = await GetPasswordRecoverService().ResetPasswordAsync(new UserApplicationSession(), "NewPassword", "NotMatchNewPassword");

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
        }

        public async Task ResetPassword_must_set_error_when_there_is_error()
        {
            CreateNewMockServices();
            var errors = new List<string> { "Error1" };
            var identityResult = new IdentityResult(errors);
            var newPassword = "NewPassword";
            _userServiceMock.Setup(c => c.GetUserAsync(It.IsAny<string>())).ReturnsAsync(new UserModel());
            _userServiceMock.Setup(c => c.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResult);

            var model = await GetPasswordRecoverService().ResetPasswordAsync(new UserApplicationSession(), newPassword, newPassword);

            Assert.True(model.HasError);
            Assert.NotEmpty(model.ErrorMessage);
            Assert.Equal(model.ForgotPasswordStep, ForgotPasswordStep.ResetPassword);
        }

        public async Task ResetPassword_must_not_set_error_when_there_is_no_error()
        {
            CreateNewMockServices();
            var identityResult = new IdentityResult(new List<string>());
            var userId = "UserId";
            var passwordToken = "PasswordToken";
            var newPassword = "NewPassword";
            var userModel = new UserModel
            {
                Id = userId,
                PasswordRecoveryToken = passwordToken
            };
            _userServiceMock.Setup(c => c.GetUserAsync(It.IsAny<string>())).ReturnsAsync(userModel);
            _userServiceMock.Setup(c => c.ChangePassword(userId, passwordToken, newPassword)).ReturnsAsync(identityResult);

            var model = await GetPasswordRecoverService().ResetPasswordAsync(new UserApplicationSession(), newPassword, "ConfirmNewPassword");

            Assert.False(model.HasError);
            Assert.Null(model.ErrorMessage);
            _userServiceMock.Verify();
        }

        private PasswordRecoverService GetPasswordRecoverService()
        {
            return new PasswordRecoverService(_phoneServiceMock.Object, _userServiceMock.Object, _auditServiceMock.Object);
        }

        private void CreateNewMockServices()
        {
            _userServiceMock = new Mock<IUserService>();
            _phoneServiceMock = new Mock<IPhoneService>();
            _auditServiceMock = new Mock<IAuditService>();
        }
    }
}
