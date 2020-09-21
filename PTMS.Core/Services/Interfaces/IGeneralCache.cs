using System;
using System.Threading.Tasks;
using PTMS.Core.Models;
using PTMS.Core.Repositories;

namespace PTMS.Core.Services.Interfaces {
    public interface IGeneralCache<T> where T : IEntity<string>
	{
		Task<T> Get(IBaseRepository<T> _templates, string key);
		Task Set(string key, T obj);
		Task Remove(string key);
		Task Restore(IBaseRepository<T> _templates);
    }
}
