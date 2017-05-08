using Embarr.WebAPI.AntiXss;
using System.Collections.Generic;
using Voyage.Core;

namespace Voyage.Models
{
    public class ForgotPasswordModel
    {
        public bool HasError { get; set; }

        [AntiXss]
        public string ErrorMessage { get; set; }

        public ForgotPasswordStep ForgotPasswordStep { get; set; }

        [AntiXss]
        public List<string> Questions { get; set; }

        [AntiXss]
        public string PasswordRecoveryToken { get; set; }

        [AntiXss]
        public string UserId { get; set; }
    }
}