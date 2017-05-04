using Embarr.WebAPI.AntiXss;

namespace Voyage.Web.Models
{
    public class UserApplicationSession
    {
        [AntiXss]
        public string UserId { get; set; }

        [AntiXss]
        public string PasswordRecoveryToken { get; set; }
    }
}