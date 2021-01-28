using PTMS.Client.SDK.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Client.SDK
{
    public interface IPTMSClient
    {
        Task<IEnumerable<string>> GetCategories();
        Task<IEnumerable<Template>> GetTemplatesByCategory(string categoryName);
        Task<IEnumerable<Template>> GetAllTemplates();
        Task<string> GetTemplate(string version, string templateId, string jsonData);
        Task<string> GetTemplate<T>(string version, string templateId, T obj);
        Task<TemplateItem> GetTemplateById(string templateId);
        Task<string> CreateTemplate(string name, string description, string category, string template);
        Task UpdateTemplate(string templateId, string name, string description, string category, string template);
        Task DeleteTemplateById(string templateId);

    }
}
