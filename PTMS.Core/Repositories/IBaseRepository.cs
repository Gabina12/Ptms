using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.Core.Models;

namespace PTMS.Core.Repositories {
    public interface IBaseRepository<T> where T : IEntity<string> {
		Task<T> Get(string id);
		Task<IEnumerable<T>> Get();
	}
}
