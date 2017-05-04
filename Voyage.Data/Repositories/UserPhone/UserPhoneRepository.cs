﻿using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using PhoneNumbers;

namespace Voyage.Data.Repositories.UserPhone
{
    public class UserPhoneRepository : BaseRepository<Models.Entities.UserPhone>, IUserPhoneRepository
    {
        public UserPhoneRepository(IVoyageDataContext context)
            : base(context)
        {
        }

        public override Models.Entities.UserPhone Add(Models.Entities.UserPhone model)
        {
            return model;
        }

        /// <summary>
        /// Deletes a phone number from the database
        /// </summary>
        /// <param name="id">Phone number ID</param>
        public override void Delete(object id)
        {
            var entity = Context.UserPhones.Find(id);
            Context.UserPhones.Remove(entity);
        }

        /// <summary>
        /// Fetch a phone number ID
        /// </summary>
        /// <param name="id">Phone number ID</param>
        /// <returns>Phone Number</returns>
        public override Models.Entities.UserPhone Get(object id)
        {
            return Context.UserPhones.Find(id);
        }

        public override IQueryable<Models.Entities.UserPhone> GetAll()
        {
            throw new NotImplementedException("Phone numbers are loaded via the user object");
        }

        public override Models.Entities.UserPhone Update(Models.Entities.UserPhone model)
        {
            Context.UserPhones.AddOrUpdate(model);
            Context.SaveChanges();
            return model;
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
        /// Return E164 formated phone number. Throw Exception if phone number is not valid
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetE164Format(string phoneNumber)
        {
            var formatedPhoneNumber = string.Empty;
            var isValidPhoneNumber = IsValidPhoneNumber(phoneNumber, out formatedPhoneNumber);

            if (!isValidPhoneNumber)
                throw new Exception("Invalid phone number.");

            return formatedPhoneNumber;
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
    }
}
