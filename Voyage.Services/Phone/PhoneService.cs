using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using PhoneNumbers;
using Voyage.Data.Repositories.UserPhone;

namespace Voyage.Services.Phone
{
    public class PhoneService : IPhoneService
    {
        private readonly IUserPhoneRepository _phoneRepository;

        public PhoneService(IUserPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        public string GenerateSecurityCode()
        {
            var random = new Random();
            return random.Next(000000, 999999).ToString();
        }

        public void InsertSecurityCode(int phoneId, string code)
        {
            var userPhone = _phoneRepository.Get(phoneId);
            userPhone.VerificationCode = code;
            _phoneRepository.Update(userPhone);
            _phoneRepository.SaveChanges();
        }

        public void ResetSecurityCode(int phoneId)
        {
            var userPhone = _phoneRepository.Get(phoneId);

            // reset verification code to anything other than previous code that has been used
            userPhone.VerificationCode = GenerateSecurityCode();
            _phoneRepository.Update(userPhone);
            _phoneRepository.SaveChanges();
        }

        public bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phone = phoneNumberUtil.Parse(phoneNumber, "US");
            formatedPhoneNumber = phoneNumberUtil.Format(phone, PhoneNumberFormat.E164);

            return phoneNumberUtil.IsValidNumber(phone);
        }

        public async Task SendSecurityCode(string phoneNumber, string securityCode)
        {
            var credential = new BasicAWSCredentials(ConfigurationManager.AppSettings.Get("AwsAccessKey"), ConfigurationManager.AppSettings.Get("AwsSecretKey"));
            var client = new AmazonSimpleNotificationServiceClient(credential, RegionEndpoint.USEast1);

            var publishRequest = new PublishRequest
            {
                Message = "Security Code: " + securityCode,
                PhoneNumber = phoneNumber
            };

            await client.PublishAsync(publishRequest);
        }
    }
}
