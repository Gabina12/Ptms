using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Repositories {
    public interface IPartialsRepository : IBaseRepository<PartialTemplate> {
		Task<PartialTemplate> Create(PartialTemplate template);
		Task Update(string id, PartialTemplate template);
		Task Delete(string id);
	}
}
