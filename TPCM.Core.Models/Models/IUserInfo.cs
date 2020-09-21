using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models
{
	public interface IUserInfo
	{
        [JsonPropertyName("created")]
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonPropertyName("creator")]
        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonPropertyName("editor")]
        [JsonProperty("editor")]
        public string Editor { get; set; }

        [JsonPropertyName("updated")]
        [JsonProperty("updated")]
        public long Updated { get; set; }
    }
}
