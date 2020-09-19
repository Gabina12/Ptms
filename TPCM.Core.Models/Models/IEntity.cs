﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TPCM.Core.Models {
    public interface IEntity<T>
	{
		[JsonPropertyName("_id")]
		[JsonProperty("_id")]
		public T Id { get; }
	}
}
