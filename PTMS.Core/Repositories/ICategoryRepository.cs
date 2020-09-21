using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.Core.Models;

namespace PTMS.Core.Repositories
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
