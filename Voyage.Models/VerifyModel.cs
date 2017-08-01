using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class VerifyModel
    {
        [AntiXss]
        public string Code { get; set; }
    }
}
