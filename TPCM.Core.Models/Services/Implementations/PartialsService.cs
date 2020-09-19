﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;
using TPCM.Core.Repositories;
using TPCM.Core.Services.Interfaces;

namespace TPCM.Core.Services.Implementations {
    public class PartialsService : IPartialsService {
		private readonly ICache<PartialCacheItem> _cache;
		private readonly IPartialsRepository _templates;
		private readonly ICategoryRepository _categories;

		public PartialsService(ICache<PartialCacheItem> cache, IPartialsRepository templates, ICategoryRepository categories) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_templates = templates ?? throw new ArgumentNullException(nameof(templates));
			_categories = categories ?? throw new ArgumentNullException(nameof(categories));
		}

		public async Task<PartialTemplate> Create(PartialTemplate template) {
			if (string.IsNullOrWhiteSpace(template.Id)) template.Id = Guid.NewGuid().ToString("N").ToLower();
			var created = await _templates.Create(template);
			await _cache.Appand("partials", new PartialCacheItem { Id = created.Id, Template = created.TemplateBody });
			return template;
		}

		public async Task Delete(string id) {
			await _templates.Delete(id);
			await _cache.Remove("partials",id);
		}

		public async Task<PartialTemplate> Get(string id) => await _templates.Get(id);

		public async Task<IResponse<IEnumerable<PartialTemplate>>> Get() => new Response<IEnumerable<PartialTemplate>>(await _templates.Get());

		public async Task Update(string id, PartialTemplate template) {
			var old = await _templates.Get(id);
			if (old == null) throw new Exception("Template not found!");
			template.Updated = (long)DateTime.UtcNow
			   .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
			   .TotalMilliseconds;
			await _templates.Update(id, template);
			await _cache.Remove("partials",id);
			await _cache.Appand("partials", new PartialCacheItem { Id = template.Id, Template = template.TemplateBody });
		}
	}
}
