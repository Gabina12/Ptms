using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models {
    public interface IVersionControl {
        [JsonPropertyName("versionControl")]
        [JsonProperty("versionControl")]
        Version[] Versions { get; }
    }

}
