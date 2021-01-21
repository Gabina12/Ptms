using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTMS.Client.SDK.Models
{

    public class TemplateItem
    {
        public TemplateItem()
        {

        }
        public TemplateItem(string id, string name, string descrip, string category, string template)
        {
            Id = id;
            Name = name;
            Description = descrip;
            Category = category;
            Template = template;
        }

        private string _id;
        [JsonProperty("_id")]
        public string Id
        {
            get
            {
                return string.IsNullOrWhiteSpace(_id) ? null : _id;
            }
            set
            {
                _id = value;
            }
        }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("options")]
        public Options Options
        {
            get
            {
                return new Options();
            }
        }
    }

    public class CreatedResponse
    {
        public string Id { get; set; }
    }

    public class Options
    {
        public int scale { get; set; } = 1;
        public bool displayHeaderFooter { get; set; } = false;
        public bool printBackground { get; set; } = false;
        public Margin margin { get; set; } = new Margin();
        public bool landscape { get; set; } = false;
        public string format { get; set; } = "Letter";
        public string headerTemplate { get; set; } = "<span style=\"font-size: 30px; width: 200px; height: 200px; background-color: black; color: white; margin: 20px;\">Header</span>";
        public string footerTemplate { get; set; } = "<span style=\"font-size: 30px; width: 50px; height: 50px; background-color: red; color:black; margin: 20px;\">Footer</span>";
    }

    public class Margin
    {
        public string top { get; set; } = "0px";
        public string bottom { get; set; } = "0px";
        public string right { get; set; } = "0px";
        public string left { get; set; } = "0px";
    }
}
