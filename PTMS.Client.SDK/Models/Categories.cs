using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTMS.Client.SDK.Models
{
    public class Categories
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("data")]
        public string[] Data { get; set; }
    }
}
