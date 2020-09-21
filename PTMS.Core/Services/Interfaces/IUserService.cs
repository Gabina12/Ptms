using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.Core.Models;

namespace PTMS.Core.Services.Interfaces {
    public interface IUserService {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}
