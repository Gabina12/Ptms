using Bondx.Extention.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TPCM.Core.Models;
using TPCM.Core.Services.Interfaces;

namespace TPCM.Core.Services.Implementations.DistributedCacheImpl {
    public class RedisCache<T> : ICache<T> where T : IEntity<string> {

        private readonly IRedisStore _cache;
        public RedisCache(IRedisStore redisStore) {
            _cache = redisStore ?? throw new ArgumentNullException(nameof(redisStore));
        }

        public async Task Appand(string key, T obj) {
            var cachedItems = await _cache.Get<List<T>>(key);
            if (cachedItems == null)
                cachedItems = new List<T>();
            else if (cachedItems.Any(x => x.Id == obj.Id)) {
                var item = cachedItems.First(x => x.Id == obj.Id);
                cachedItems.Remove(item);
            }
            cachedItems.Add(obj);
            await _cache.Remove(key);
            await _cache.Set(key, cachedItems, null);
        }

        public async Task<T> Get(string key) {
            return await _cache.Get<T>(key);
        }

        public async Task<IEnumerable<T>> GetList(string key) {
           return await _cache.Get<IEnumerable<T>>(key);
        }

        public async Task Remove(string key) {
            await _cache.Remove(key);
        }

        public async Task Remove(string key, string id) {
            if (!((await _cache.Get<T>(key)) is List<T> cachedItems)) return;

            var toRemove = cachedItems.FirstOrDefault(x => x.Id == id);
            cachedItems.Remove(toRemove);
            await _cache.Remove(key);
            await _cache.Set(key, cachedItems, null);
        }

        public async Task Set(string ket, T obj) {
            await _cache.Set(ket, obj, null);
        }
    }
}
