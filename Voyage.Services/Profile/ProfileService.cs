using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.ProfileImage;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.User;

namespace Voyage.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;
        private readonly IProfileImageRepository _profileImageRepository;

        public ProfileService(IUserService userService, IProfileImageRepository profileImageRepository)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
            _profileImageRepository = profileImageRepository.ThrowIfNull(nameof(profileImageRepository));
        }

        public async Task<CurrentUserModel> GetCurrentUserAync(string userId)
        {
            var user = await _userService.GetUserAsync(userId);
            var roles = await _userService.GetUserRolesAsync(userId);
            var profileImage = GetProfileImage(userId);
            return new CurrentUserModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phones = user.Phones,
                Roles = roles.Select(_ => _.Name),
                ProfileImage = profileImage
            };
        }

        public async Task<CurrentUserModel> UpdateProfileAsync(string userId, ProfileModel model)
        {
            var userModel = await _userService.GetUserAsync(userId);
            userModel.Email = model.Email;
            userModel.FirstName = model.FirstName;
            userModel.LastName = model.LastName;
            userModel.Phones = model.Phones;
            await _userService.UpdateUserAsync(userId, userModel);

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var result = await _userService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);
                if (result.Errors.Any())
                {
                    throw new BadRequestException(Constants.ErrorCodes.InvalidPassword, result.Errors.First());
                }
            }

            if (string.IsNullOrEmpty(model.ProfileImage))
            {
                return await GetCurrentUserAync(userId);
            }

            var currentImage = _profileImageRepository.GetAll()
                .FirstOrDefault(_ => _.UserId == userId);

            if (currentImage != null)
            {
                currentImage.ImageData = model.ProfileImage;
            }
            else
            {
                currentImage = new ProfileImage
                {
                    UserId = userId,
                    ImageData = model.ProfileImage
                };
            }

            await _profileImageRepository.UpdateAsync(currentImage);

            return await GetCurrentUserAync(userId);
        }

        public string GetProfileImage(string userId)
        {
            var currentImage = _profileImageRepository.GetAll()
                .FirstOrDefault(_ => _.UserId == userId);
            return currentImage?.ImageData;
        }

        public async Task GetInitialProfileImageAsync(string userId, string emailAddress)
        {
            try
            {
                var request = WebRequest.Create($"http://picasaweb.google.com/data/entry/api/user/{emailAddress}?alt=json");
                using (var response = await request.GetResponseAsync())
                {
                    var dataStream = response.GetResponseStream();
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        var obj = JObject.Parse(responseFromServer);
                        var thumbnail = (string)obj["entry"]["gphoto$thumbnail"]["$t"];
                        if (string.IsNullOrEmpty(thumbnail))
                            return;

                        using (var webClient = new WebClient())
                        {
                            byte[] data = webClient.DownloadData(thumbnail);
                            string base64ImageRepresentation = Convert.ToBase64String(data);

                            if (string.IsNullOrEmpty(base64ImageRepresentation))
                                return;

                            await _profileImageRepository.AddAsync(new ProfileImage
                            {
                                UserId = userId,
                                ImageData = $"data:image/jpeg;base64,{base64ImageRepresentation}"
                            });
                        }
                    }
                }
            }
            catch
            {
                // We don't care if it fails. We'll just show the default profile image.
            }
        }
    }
}