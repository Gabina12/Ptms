using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PTMS.Core.Models;
using PTMS.Core.Services.Interfaces;

namespace PTMS.Core.Services.Implementations {
    public class MemCache<T> : ICache<T> where T : IEntity<string> {
		private readonly IMemoryCache _cache;
        private object sync = new object();
		public MemCache(IMemoryCache cache) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

        public Task Appand(string key, T obj) {
            lock (sync) {
                if (!(_cache.Get(key) is List<T> cachedItems))
                    cachedItems = new List<T>();
                cachedItems.Add(obj);
                _cache.Remove(key);
                _cache.Set(key, cachedItems);
            }
            return Task.CompletedTask;
        }

        public Task<T> Get(string key) {
            var item = _cache.Get<T>(key);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<T>> GetList(string key) {
            var cachedItems = _cache.Get(key) as IEnumerable<T>;
            return Task.FromResult(cachedItems);
        }

        public Task Remove(string key) {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task Remove(string key, string id) {
            lock (sync) {
                if (!(_cache.Get(key) is List<T> cachedItems)) return Task.CompletedTask;

                var toRemove = cachedItems.FirstOrDefault(x => x.Id == id);
                cachedItems.Remove(toRemove);
                _cache.Remove(key);
                _cache.Set(key, cachedItems);
            }
            return Task.CompletedTask;
        }

        public Task Set(string ket, T obj) {
            _cache.Set(ket, obj);
            return Task.CompletedTask;
        }
    }
}
