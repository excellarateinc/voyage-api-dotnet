using Voyage.Core.Exceptions;

namespace Voyage.Web.Models
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; }

        public NotFoundException NotFoundException { get; set; }
    }
}