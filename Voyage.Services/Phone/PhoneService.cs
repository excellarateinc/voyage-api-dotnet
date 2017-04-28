using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using PhoneNumbers;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Services.User;

namespace Voyage.Services.Phone
{
    public class PhoneService : IPhoneService
    {
        private readonly IUserPhoneRepository _phoneRepository;
        private readonly IUserService _userService;

        public PhoneService(IUserPhoneRepository phoneRepository, IUserService userService)
        {
            _phoneRepository = phoneRepository;
            _userService = userService;
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
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phone = phoneNumberUtil.Parse(phoneNumber, "US");
            formatedPhoneNumber = phoneNumberUtil.Format(phone, PhoneNumberFormat.E164);

            return phoneNumberUtil.IsValidNumber(phone);
        }

        /// <summary>
        /// validate and if valid phone number send security code to a give user phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public async Task SendSecurityCode(string phoneNumber, string securityCode)
        {
            var formatedPhoneNumber = string.Empty;
            if (IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber))
            {
                var credential = new BasicAWSCredentials(ConfigurationManager.AppSettings.Get("AwsAccessKey"), ConfigurationManager.AppSettings.Get("AwsSecretKey"));
                var client = new AmazonSimpleNotificationServiceClient(credential, RegionEndpoint.USEast1);

                var publishRequest = new PublishRequest
                {
                    Message = "Security Code: " + securityCode,
                    PhoneNumber = formatedPhoneNumber
                };

                await client.PublishAsync(publishRequest);
            }
        }

        /// <summary>
        /// validate security code sent to user phone
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public async Task<bool> IsValidSecurityCode(string userId, string securityCode)
        {
            var user = await _userService.GetUserAsync(userId);
            var phone = user.Phones.FirstOrDefault(c => c.VerificationCode == securityCode);
            return phone != null;
        }
    }
}
