using Embarr.WebAPI.AntiXss;

namespace Voyage.Models
{
    public class ApplicationInfoModel
    {
        [AntiXss]
        public string BuildNumber { get; set; }
    }
}
