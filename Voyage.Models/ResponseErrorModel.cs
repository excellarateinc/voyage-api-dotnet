using Embarr.WebAPI.AntiXss;
using Newtonsoft.Json;

namespace Voyage.Models
{
    public class ResponseErrorModel
    {
        [AntiXss]
        public string Error { get; set; }

        [AntiXss]
        [JsonIgnore]
        public string Field { get; set; }

        [AntiXss]
        public string ErrorDescription { get; set; }
    }
}
