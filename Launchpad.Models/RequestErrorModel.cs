using Newtonsoft.Json;

namespace Launchpad.Models
{
    public class ResponseErrorModel
    {
        public string Error { get; set; }

        [JsonIgnore]
        public string Field { get; set; }

        public string ErrorDescription { get; set; }
    }
}
