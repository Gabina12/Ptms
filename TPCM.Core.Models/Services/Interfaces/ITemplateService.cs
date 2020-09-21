using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Services.Interfaces {
    public interface ITemplateService
	{
		Task<Template> Create(Template template);
		Task Update(string id, Template template);
		Task Delete(string id);
		Task<Template> Get(string id);
		Task<IResponse<IEnumerable<Template>>> Get();
	}
}
