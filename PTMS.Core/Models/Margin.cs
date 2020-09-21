using Newtonsoft.Json;

namespace PTMS.Core.Models {
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Margin {
        [JsonProperty("top")]
        public string Top { get; set; }

        [JsonProperty("bottom")]
        public string Bottom { get; set; }

        [JsonProperty("right")]
        public string Right { get; set; }

        [JsonProperty("left")]
        public string Left { get; set; }
    }
}
