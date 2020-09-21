using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Services.Interfaces {
    public interface IPartialsService {
		Task<PartialTemplate> Create(PartialTemplate template);
		Task Update(string id, PartialTemplate template);
		Task Delete(string id);
		Task<PartialTemplate> Get(string id);
		Task<IResponse<IEnumerable<PartialTemplate>>> Get();
	}
}
