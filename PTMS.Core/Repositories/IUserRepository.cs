using System.Threading.Tasks;
using PTMS.Core.Models;

namespace PTMS.Core.Repositories {
    public interface IUserRepository : IBaseRepository<User> {
		Task<User> Create(User template);
		Task Update(string id, User template);
		Task Delete(string id);
		Task<User> Get(string userName, string password);
		Task Migrate();

	}
}
