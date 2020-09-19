using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPCM.Core.Extentions;
using TPCM.Core.Models;
using TPCM.Core.Repositories;
using TPCM.Core.Services.Interfaces;

namespace TPCM.Core.Services.Implementations {
    public class UserService : IUserService {
        private readonly IUserRepository _users;

        public UserService(IUserRepository users) {
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<User> Authenticate(string username, string password) {
            var user = await _users.Get(username,password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<User>> GetAll() {
            return (await _users.Get()).Select(x => x.WithoutPassword());
        }
    }
}
