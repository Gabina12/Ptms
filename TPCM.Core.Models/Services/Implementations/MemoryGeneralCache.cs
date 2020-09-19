using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TPCM.Core.Models;
using TPCM.Core.Repositories;
using TPCM.Core.Services.Interfaces;

namespace TPCM.Core.Services.Implementations {
    public class MemoryGeneralCache<T> : IGeneralCache<T> where T : IEntity<string>, ITemplateInfo, IVersionControl {
		private readonly IMemoryCache _cache;

		public MemoryGeneralCache(IMemoryCache cache) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<T> Get(IBaseRepository<T> _templates, string key) {
			return await _cache.GetOrCreateAsync(key, async entry => {
				entry.SlidingExpiration = TimeSpan.FromDays(365);
                string[] keys = key.Split('_');
                var item = await _templates.Get(keys[0]);
				if (item.Version != keys[1] && !string.IsNullOrWhiteSpace(keys[1]))
					item.TemplateBody = item.Versions.FirstOrDefault(x => x.VersionNumber == keys[1]).TemplateBody;
				return item;
			});
		}

		public Task Remove(string key) {
			_cache.Remove(key);
			return Task.CompletedTask;
		}

		public async Task Restore(IBaseRepository<T> _templates) {
			var templates = await _templates.Get();
			foreach (var item in templates) {
				_cache.Set(item.Id, item);
			}
		}

		public Task Set(string key, T obj) {
			_cache.Set(key, obj);
			return Task.CompletedTask;
		}

	}
}
