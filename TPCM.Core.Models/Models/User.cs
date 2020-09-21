using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models {
    public class User : IEntity<string>, IUserInfo {
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public string Id { get; set; }

		[JsonPropertyName("user")]
		[JsonProperty("user")]
		public string UserName { get; set; }

		[JsonPropertyName("password")]
		[JsonProperty("password")]
		public string Password { get; set; }

		[JsonPropertyName("role")]
		[JsonProperty("role")]
		public string Role { get; set; }

        public long Created { get; set; }

        public string Creator { get; set; }

        public string Editor { get; set; }

        public long Updated { get; set; }
    }
}
