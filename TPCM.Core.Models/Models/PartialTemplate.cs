using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models
{
	public class PartialTemplate : IEntity<string>, ITemplateInfo, IUserInfo
	{
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [JsonPropertyName("_rev")]
        [JsonProperty("_rev")]
        public string Rev { get; }

        public string Name { get; set; }

		public string Description { get; set; }

		public string TemplateBody { get; set; }

        public long Created { get; set; }

        public string Creator { get; set; }

        public long Updated { get; set; }
        public string Editor { get; set; }

        [JsonPropertyName("category")]
        [JsonProperty("category")]
        public string Category { get; set; }

        public string Version { get; set; }
    }
}
