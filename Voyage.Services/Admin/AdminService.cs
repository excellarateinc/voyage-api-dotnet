using System;
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

        public async Task<UserModel> ToggleAccountStatus(string userId, ChangeAccountStatusModel changeAccountStatusModel)
        {
            if (string.IsNullOrEmpty(userId))
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "Empty User Id");
            try
            {
                var user = await _userService.GetUserAsync(userId);
                user.IsActive = changeAccountStatusModel.IsActive == null ? user.IsActive : (bool)changeAccountStatusModel.IsActive;
                user.IsVerifyRequired = changeAccountStatusModel.IsVerifyRequired == null ? user.IsVerifyRequired : (bool)changeAccountStatusModel.IsVerifyRequired;
                var result = await _userService.UpdateUserAsync(userId, user);
                return result;
            }
            catch (Exception)
            {
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "Invalid Request.");
            }
        }
    }
}
