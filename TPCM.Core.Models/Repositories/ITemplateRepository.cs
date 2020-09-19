﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Repositories {
    public interface ITemplateRepository : IBaseRepository<Template>
	{
		Task<Template> Create(Template template);
		Task Update(string id, Template template);
		Task Delete(string id);
	}
}
