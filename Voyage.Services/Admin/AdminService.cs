using System.Net;
using System.Threading.Tasks;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Models;
using Voyage.Services.User;

namespace Voyage.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUserService _userService;

        public AdminService(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        /// <summary>
        /// To toggle the account status of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changeAccountStatusModel"></param>
        /// <returns>UserModel</returns>
        public async Task<UserModel> ToggleAccountStatus(string userId, ChangeAccountStatusModel changeAccountStatusModel)
        {
            if (string.IsNullOrEmpty(userId))
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "Empty User Id");

            var user = await _userService.GetUserAsync(userId);

            if (changeAccountStatusModel.IsActive.HasValue)
                user.IsActive = changeAccountStatusModel.IsActive.Value;

            if (changeAccountStatusModel.IsVerifyRequired.HasValue)
                user.IsVerifyRequired = changeAccountStatusModel.IsVerifyRequired.Value;

            var result = await _userService.UpdateUserAsync(userId, user);
            return result;
        }
    }
}
