using Newtonsoft.Json;
using PTMS.Client.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PTMS.Client.SDK
{
    public class PTMSClient : IPTMSClient
    {

        private readonly HttpClient _http;

        public PTMSClient(IHttpClientFactory facory)
        {
            _http = facory.CreateClient("ptms-client");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                         SecurityProtocolType.Tls11 |
                                         SecurityProtocolType.Tls12;

        }

        public async Task<string> CreateTemplate(string name, string description, string category, string template)
        {
            var templateItem = new TemplateItem(null, name, description, category, template);
            string jsonData = JsonConvert.SerializeObject(templateItem);
            var response = await _http.PutAsync($"/api/templates", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();
            return JsonConvert.DeserializeObject<TemplateBody<CreatedResponse>>(result).Data.Id;
        }

        public async Task UpdateTemplate(string templateId, string name, string description, string category, string template)
        {
            var templateItem = new TemplateItem(templateId, name, description, category, template);
            string jsonData = JsonConvert.SerializeObject(templateItem);
            var response = await _http.PostAsync($"/api/templates/{templateId}", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            _ = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();
        }

        public async Task<IEnumerable<string>> GetCategories()
        {
            var response = await _http.GetAsync("/api/categories/templates");
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();

            return JsonConvert.DeserializeObject<Categories>(result).Data;
        }

        public async Task<string> GetTemplate(string version, string templateId, string jsonData)
        {
            var response = await _http.PostAsync($"/api/templates/{templateId}/render/txt/{version}", new StringContent(jsonData, Encoding.UTF8, "application/json"));

            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();

            return result;
        }

        public async Task<string> GetTemplate<T>(string version, string templateId, T obj)
        {
            string jsonData = JsonConvert.SerializeObject(obj);
            var response = await _http.PostAsync($"/api/templates/{templateId}/render/txt/{version}", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();

            return result;
        }

        public async Task<IEnumerable<Template>> GetTemplatesByCategory(string categoryName)
        {
            var response = await _http.GetAsync("/api/templates");
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();

            return JsonConvert.DeserializeObject<TemplateList>(result).Data.Where(x => x.Category == categoryName).ToList();
        }

        public async Task<TemplateItem> GetTemplateById(string templateId)
        {
            var response = await _http.GetAsync($"/api/templates/{templateId}");
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();
            return JsonConvert.DeserializeObject<TemplateBody<TemplateItem>>(result).Data;
        }

        public async Task DeleteTemplateById(string templateId)
        {
            var response = await _http.DeleteAsync($"/api/templates/{templateId}");
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();
        }

        public async Task<IEnumerable<Template>> GetAllTemplates()
        {
            var response = await _http.GetAsync("/api/templates");
            var result = await response.Content?.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException();

            return JsonConvert.DeserializeObject<TemplateList>(result).Data;
        }
    }
}
