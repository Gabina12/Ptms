using Newtonsoft.Json;

namespace PTMS.Client.SDK.Models
{
    public class TemplateBody<T>
    {
        public TemplateBody()
        {

        }
        public TemplateBody(T data)
        {
            Data = data;
        }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
