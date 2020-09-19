using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Services.Interfaces {
    public interface ICache<T> where T : IEntity<string> {
		Task<T> Get(string key);
		Task<IEnumerable<T>> GetList(string key);
		Task Set(string ket, T obj);
		Task Remove(string key);
		Task Appand(string key, T obj);
		Task Remove(string key, string id);
    }
}
