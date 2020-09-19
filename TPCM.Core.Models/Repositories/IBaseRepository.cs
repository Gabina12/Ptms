using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Repositories {
    public interface IBaseRepository<T> where T : IEntity<string> {
		Task<T> Get(string id);
		Task<IEnumerable<T>> Get();
	}
}
