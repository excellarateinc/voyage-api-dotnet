using Voyage.Core.Exceptions;

namespace Voyage.Security.Oauth2.Models
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; }

        public NotFoundException NotFoundException { get; set; }
    }
}