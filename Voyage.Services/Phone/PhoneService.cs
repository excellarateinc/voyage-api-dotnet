using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;

namespace Voyage.Services.Phone
{
    public class PhoneService : IPhoneService
    {
        private readonly IUserPhoneRepository _phoneRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public PhoneService(ApplicationUserManager userManager, IMapper mapper, IUserPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// generate security code from random set of number between 100000 to 999999
        /// </summary>
        /// <returns></returns>
        public string GenerateSecurityCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// insert security code to user phone number record
        /// </summary>
        /// <param name="phoneId"></param>
        /// <param name="code"></param>
        public async Task InsertSecurityCodeAsync(int phoneId, string code)
        {
            var userPhone = await _phoneRepository.GetAsync(phoneId);
            userPhone.VerificationCode = code;
            await _phoneRepository.UpdateAsync(userPhone);
            await _phoneRepository.SaveChangesAsync();
        }

        /// <summary>
        /// check if given phone number is valid format e.g E.164
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="formatedPhoneNumber"></param>
        /// <returns></returns>
        public bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber)
        {
            return _phoneRepository.IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber);
        }

        /// <summary>
        /// Return E164 formated phone number. Throw Exception if phone number is not valid
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetE164Format(string phoneNumber)
        {
            return _phoneRepository.GetE164Format(phoneNumber);
        }

        /// <summary>
        /// validate and if valid phone number send security code to a give user phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public async Task SendSecurityCodeAsync(string phoneNumber, string securityCode)
        {
             await _phoneRepository.SendSecurityCode(phoneNumber, securityCode);
        }

        /// <summary>
        /// get the phone numbers of the logged in user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task SendSecurityCodeToUserPhoneNumberAsync(string userName)
        {
            try
            {
                string securityCode = GenerateSecurityCode();
                var user = await _userManager.FindByNameAsync(userName);

                // Limit the number of phones that can receive a verification code to 5. This prevents an attacker from overloading
                // the list of phone numbers for a user and spamming an infinite number of phones with security codes.
                var phones = user.Phones.Where(userPhone => userPhone.PhoneType == Models.Enum.PhoneType.Mobile)
                    .Take(5).ToList();

                if (!phones.Any())
                    throw new BadRequestException("The verification phone number is invalid. Please contact technical support for assistance");

                // send the security code to the phone number and save the security code to the database
                foreach (var phone in phones)
                {
                    await SendSecurityCodeAsync(phone.PhoneNumber, securityCode);
                    await InsertSecurityCodeAsync(phone.Id, securityCode);
                }
            }
            catch (Exception)
            {
                throw new BadRequestException(HttpStatusCode.InternalServerError.ToString(), "Failure sending text message. Please contact support.");
            }
        }

        /// <summary>
        /// validate security code sent to user phone
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public async Task<bool> IsValidSecurityCodeAsync(string userId, string securityCode)
        {
            var user = await GetUserAsync(userId);
            var appuser = _userManager.FindById(userId);
            var phone = user.Phones.FirstOrDefault(c => c.VerificationCode == securityCode);

            if (phone == null)
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "The verification code provided is invalid.");

            // TODO: Throw an exception when verification code expires
            // TODO: Need to create column IsValidated and VerifyCodeExpiresOn in UserPhone table and update IsValidated to true in the DB
            appuser.IsVerifyRequired = false;

            // update the user
            await _userManager.UpdateAsync(appuser);
            return phone != null;
        }

        /// <summary>
        /// clear user phone security. This usually use after security code is verified
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task ClearUserPhoneSecurityCodeAsync(string userId)
        {
            var user = await GetUserAsync(userId);
            foreach (var userPhone in user.Phones)
            {
                if (!string.IsNullOrWhiteSpace(userPhone.VerificationCode))
                {
                    userPhone.VerificationCode = string.Empty;
                }
            }

            await UpdateUserAsync(user.Id, user);
        }

        private async Task<UserModel> GetUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null || appUser.Deleted)
                throw new Voyage.Core.Exceptions.NotFoundException($"Could not locate entity with ID {userId}");

            return _mapper.Map<UserModel>(appUser);
        }

        private async Task<UserModel> UpdateUserAsync(string userId, UserModel model)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
                throw new Voyage.Core.Exceptions.NotFoundException($"Could not locate entity with Id {userId}");

            if (!IsValidPhoneNumbers(model))
            {
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "Invalid phone number.");
            }

            _mapper.Map<UserModel, ApplicationUser>(model, appUser);

            CollectionHelpers.MergeCollection(
                _mapper,
                source: model.Phones,
                destination: appUser.Phones,
                predicate: (s, d) => s.Id == d.Id,
                deleteAction: async entity => await _phoneRepository.DeleteAsync(entity.Id));

            await _userManager.UpdateAsync(appUser);
            return _mapper.Map<UserModel>(appUser);
        }

        private bool IsValidPhoneNumbers(UserModel userModel)
        {
            // validate user phone numbers
            var isValidPhoneNumbers = true;
            foreach (var phone in userModel.Phones)
            {
                var formatedPhoneNumber = string.Empty;
                if (IsValidPhoneNumber(phone.PhoneNumber, out formatedPhoneNumber))
                {
                    phone.PhoneNumber = formatedPhoneNumber;
                }
                else
                {
                    isValidPhoneNumbers = false;
                    break;
                }
            }

            return isValidPhoneNumbers;
        }
    }
}
