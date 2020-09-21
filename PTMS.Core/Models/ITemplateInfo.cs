using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PTMS.Core.Models
{
	public interface ITemplateInfo
	{
		[JsonPropertyName("name")]
		[JsonProperty("name")]
		string Name { get; }

		[JsonPropertyName("description")]
		[JsonProperty("description")]
		string Description { get; }

		[JsonPropertyName("template")]
		[JsonProperty("template")]
		string TemplateBody { get; set; }

		[JsonPropertyName("version")]
		[JsonProperty("version")]
		string Version { get; }
	}
}
