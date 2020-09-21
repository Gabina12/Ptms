using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Extentions;
using TPCM.Core.Models;
using TPCM.Core.Repositories;
using TPCM.Core.Services.Interfaces;

namespace TPCM.Core.Services.Implementations {
	public class TemplateService : ITemplateService {
		private readonly IGeneralCache<Template> _cache;
		private readonly ITemplateRepository _templates;
		private readonly ICategoryRepository _categories;

		public TemplateService(IGeneralCache<Template> cache, ITemplateRepository templates, ICategoryRepository categories) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_templates = templates ?? throw new ArgumentNullException(nameof(templates));
			_categories = categories ?? throw new ArgumentNullException(nameof(categories));
		}

		public async Task<Template> Create(Template template) {
			if (string.IsNullOrWhiteSpace(template.Id)) template.Id = Guid.NewGuid().ToString("N").ToLower();
			template.Version = "v1";
			//template.Versions = new Models.Version[] { new Models.Version { VersionNumber = "v1", Created = DateTime.Now, Creator = "system", TemplateBody = template.TemplateBody } };
			var created = await _templates.Create(template);


			if (!(await _categories.Exists(template.Category))) {
				await _categories.Create(new Category {
					Id = Guid.NewGuid(),
					Name = template.Category,
					Description = template.Category
				});
			}

			await _cache.Set(template.ToCacheKey(), created);
			return template;
		}

		public async Task Delete(string id) {
			await _templates.Delete(id);
			await _cache.Remove(id);
		}

		public async Task<Template> Get(string id) => await _templates.Get(id);

		public async Task<IResponse<IEnumerable<Template>>> Get() => new Response<IEnumerable<Template>>(await _templates.Get());

		public async Task Update(string id, Template template) {
			var old = await _templates.Get(id);
			if (old == null) throw new Exception("Template not found!");

			var newVersion = new Models.Version {
				VersionNumber = old.Version,
				Created = DateTime.Now,
				Creator = "system",
				TemplateBody = old.TemplateBody
			};

			var versions = old.Versions;
			if (old.Version != template.Version) {
				if (versions == null) versions = new Models.Version[0];
				Array.Resize(ref versions, versions.Length + 1);
				versions[versions.Length - 1] = newVersion;
			}
			template.Versions = versions;
			template.Updated = DateTime.UtcNow.AsDate();

			await _templates.Update(id, template);
			if (old.Version == template.Version)
				await _cache.Remove(id);
			await _cache.Set(template.ToCacheKey(), template);
		}
	}
}
