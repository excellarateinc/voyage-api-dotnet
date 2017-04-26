using System;
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

        public string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(000000, 999999).ToString();
        }

        public void InsertVerificationCode(int phoneId, string code)
        {
            var userPhone = _phoneRepository.Get(phoneId);
            userPhone.VerificationCode = code;
            _phoneRepository.Update(userPhone);
            _phoneRepository.SaveChanges();
        }

        public void ResetVerificationCode(int phoneId)
        {
            var userPhone = _phoneRepository.Get(phoneId);

            // reset verification code to anything other than previous code that has been used
            userPhone.VerificationCode = GenerateVerificationCode();
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
    }
}
