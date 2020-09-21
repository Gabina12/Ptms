using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace PTMS.Core.Models {
    public class Version {
        [JsonPropertyName("version")]
        [JsonProperty("version")]
        public string VersionNumber { get; set; }

        [JsonPropertyName("creator")]
        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonPropertyName("created")]
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("template")]
        [JsonProperty("template")]
        public string TemplateBody { get; set; }
    }

}
