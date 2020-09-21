using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models {

    public class Template : IEntity<string>, IUserInfo, ITemplateInfo, IVersionControl {

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [JsonPropertyName("_rev")]
        [JsonProperty("_rev")]
        public string Rev { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long Created { get; set; }

        public string Creator { get; set; }

        public long Updated { get; set; }

        public string Editor { get; set; }

        public string TemplateBody { get; set; }

        [JsonPropertyName("caching")]
        [JsonProperty("caching")]
        public bool Caching { get; set; }

        [JsonPropertyName("loadExternalSources")]
        [JsonProperty("loadExternalSources")]
        public bool LoadExternalSources { get; set; }

        [JsonPropertyName("category")]
        [JsonProperty("category")]
        public string Category { get; set; }

        public string Version { get; set; }

        [JsonPropertyName("options")]
        [JsonProperty("options")]
        public Options Options { get; set; }

       
        [JsonPropertyName("versionControl")]
        [JsonProperty("versionControl")]
        public Version[] Versions {get;set;}

      
    }


}
