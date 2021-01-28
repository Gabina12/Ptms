using Newtonsoft.Json;

namespace PTMS.Client.SDK.Models
{
    public class TemplateList
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("data")]
        public Template[] Data { get; set; }
    }
}
