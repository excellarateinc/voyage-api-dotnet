using Voyage.Models.Enum;

namespace Voyage.Web.Models
{
    public class ForgotPasswordModel
    {
        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public ForgotPasswordStep ForgotPasswordStep { get; set; }
    }
}