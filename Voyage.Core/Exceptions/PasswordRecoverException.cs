using System.Net;

namespace Voyage.Core.Exceptions
{
    public class PasswordRecoverException : ApiException
    {
        public PasswordRecoverException(ForgotPasswordStep forgotPasswordStep, string message)
            : base(HttpStatusCode.BadRequest, message)
        {
            ForgotPasswordStep = forgotPasswordStep;
        }

        public ForgotPasswordStep ForgotPasswordStep { get; }
    }
}
