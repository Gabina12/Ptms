using Newtonsoft.Json;
using System.Collections.Generic;

namespace TPCM.Core.Models {
    public class Options {
        [JsonProperty("scale")]
        public int Scale { get; set; }

        [JsonProperty("displayHeaderFooter")]
        public bool DisplayHeaderFooter { get; set; }

        [JsonProperty("headerTemplate")]
        public string HeaderTemplate { get; set; }

        [JsonProperty("footerTemplate")]
        public string FooterTemplate { get; set; }

        [JsonProperty("printBackground")]
        public bool PrintBackground { get; set; }

        [JsonProperty("landscape")]
        public bool Landscape { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("margin")]
        public Margin Margin { get; set; }
    }

    public class PartialCacheItem : IEntity<string> {
        public string Id { get; set; }
        public string Template { get; set; }
    }

}
