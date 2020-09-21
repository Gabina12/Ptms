using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TPCM.Core.Models;

namespace TPCM.Core.Repositories
{
	public interface ICategoryRepository
	{
		Task<Category> Create(Category template);
		Task Update(Guid id, Category template);
		Task Delete(Guid id);
		Task<Category> Get(Guid id);
		Task<IEnumerable<Category>> Get();
		Task<bool> Exists(string name);
	}
}
