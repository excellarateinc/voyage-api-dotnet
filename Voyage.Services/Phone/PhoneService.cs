using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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
        /// generate security code from random set of number between 000000 to 999999
        /// </summary>
        /// <returns></returns>
        public string GenerateSecurityCode()
        {
            var random = new Random();
            return random.Next(000000, 999999).ToString();
        }

        /// <summary>
        /// insert security code to user phone number record
        /// </summary>
        /// <param name="phoneId"></param>
        /// <param name="code"></param>
        public void InsertSecurityCode(int phoneId, string code)
        {
            var userPhone = _phoneRepository.Get(phoneId);
            userPhone.VerificationCode = code;
            _phoneRepository.Update(userPhone);
            _phoneRepository.SaveChanges();
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
        public async Task SendSecurityCode(string phoneNumber, string securityCode)
        {
                var response = await _phoneRepository.SendSecurityCode(phoneNumber, securityCode);
        }

        public async Task SendSecurityCodeToUser(string userName)
        {
            string securityCode = GenerateSecurityCode();
            var user = await GetUserAsync("aa1aab77-570e-41fc-9c2d-ae8fd843f046");

            // Limit the number of phones that can receive a verification code to 5. This prevents an attacker from overloading
            // the list of phone numbers for a user and spamming an infinite number of phones with security codes.
            var phones = user.Phones.Where(userPhone => userPhone.PhoneType == Models.Enum.PhoneType.Mobile)
                .Select(mobilePhone => mobilePhone.PhoneNumber).Take(5).ToList();
        if (!phones.Any())
            throw new BadRequestException("The verification phone number is invalid. Please contact technical support for assistance");

        user.IsVerifyRequired = true;

        foreach (var phone in phones)
        {
               await SendSecurityCode(phone, securityCode);
        }

            // throw new exception    Failure sending text message. Please contact support.'
        }

        /// <summary>
        /// validate security code sent to user phone
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public async Task<bool> IsValidSecurityCode(string userId, string securityCode)
        {
            var user = await GetUserAsync(userId);
            var phone = user.Phones.FirstOrDefault(c => c.VerificationCode == securityCode);
            return phone != null;
        }

        /// <summary>
        /// clear user phone security. This usually use after security code is verified
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task ClearUserPhoneSecurityCode(string userId)
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
                deleteAction: entity => _phoneRepository.Delete(entity.Id));

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
