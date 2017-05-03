using System.Collections.Generic;
using Voyage.Core;

namespace Voyage.Models
{
    public class ForgotPasswordModel
    {
        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public ForgotPasswordStep ForgotPasswordStep { get; set; }

        public List<string> Questions { get; set; }

        public string PasswordRecoveryToken { get; set; }

        public string UserId { get; set; }
    }
}