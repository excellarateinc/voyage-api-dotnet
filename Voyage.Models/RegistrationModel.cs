﻿using FluentValidation.Attributes;
using System.Collections.Generic;
using Voyage.Models.Validators;

namespace Voyage.Models
{
    [Validator(typeof(RegistrationModelValidator))]
    public class RegistrationModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<UserPhoneModel> PhoneNumbers { get; set; }
    }
}
