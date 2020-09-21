using Bondx.Extention.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMS.Core.Extentions;
using PTMS.Core.Models;
using PTMS.Core.Services.Interfaces;

namespace PTMS.Core.Services.Implementations.DistributedCacheImpl {
    public class RedisGeneralCache<T> : IGeneralCache<T> where T : IEntity<string>, ITemplateInfo, IVersionControl {

        private readonly IRedisStore _redisStore;

        public RedisGeneralCache(IRedisStore redisStore) {
            _redisStore = redisStore ?? throw new ArgumentNullException(nameof(redisStore));
        }

        public async Task<T> Get(Repositories.IBaseRepository<T> _templates, string key) {
            string[] keys = key.Split('_');
            var template = await _redisStore.Get<T>(key);
            if(template == null) {
                var item = await _templates.Get(keys[0]);
                if (item.Version != keys[1] && !string.IsNullOrWhiteSpace(keys[1]))
                    item.TemplateBody = item.Versions.FirstOrDefault(x => x.VersionNumber == keys[1]).TemplateBody;
                return item;
            }

            return template;
        }

        public async Task Remove(string key) {
            await _redisStore.Remove(key);
        }

        public async Task Restore(Repositories.IBaseRepository<T> _templates) {
            var templates = await _templates.Get();
            foreach (var item in templates) {
                await _redisStore.Set(item.Id.ToCacheKey(""), item, null);
            }
        }

        public async Task Set(string key, T obj) {
            await _redisStore.Set(key, obj, null);
        }
    }
}
