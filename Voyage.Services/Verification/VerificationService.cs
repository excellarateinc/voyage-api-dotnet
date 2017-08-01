using System.Linq;
using System.Threading.Tasks;
using Voyage.Core.Exceptions;
using Voyage.Services.Phone;
using Voyage.Services.User;

namespace Voyage.Services.Verification
{
    public class VerificationService : IVerificationService
    {
        private readonly IPhoneService _phoneService;
        private readonly IUserService _userService;

        public VerificationService(IPhoneService phoneService, IUserService userService)
        {
            _phoneService = phoneService;
            _userService = userService;
        }

        /// <summary>
        /// Send a code to the user's phone for verification.
        /// </summary>
        /// <param name="userId">The user id of the user to send the code to.</param>
        /// <returns></returns>
        public async Task SendCode(string userId)
        {
            // validate user and phone number and send verification code
            var user = await _userService.GetUserAsync(userId);
            var userPhoneNumber = user.Phones.FirstOrDefault();
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
                await _phoneService.SendSecurityCodeAsync(userPhoneNumber.PhoneNumber, securityCode);
            }
        }

        /// <summary>
        /// Validate security code.
        /// </summary>
        /// <param name="userId">The user id of the user to verify.</param>
        /// <param name="code">The code sent to the user's phone.</param>
        /// <returns></returns>
        public async Task VerifyCodeAsync(string userId, string code)
        {
            // check for empty security code
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BadRequestException("Security code is a required field.");
            }

            if (!await _phoneService.IsValidSecurityCodeAsync(userId, code))
            {
                throw new BadRequestException("Provided code is invalid.");
            }

            await _phoneService.ClearUserPhoneSecurityCodeAsync(userId);
        }
    }
}
