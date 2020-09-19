using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TPCM.Core.Models;
using TPCM.Core.Repositories;

namespace TPCM.Infrastructure {
    public class UserRepository : IUserRepository {
		private readonly IMongoCollection<User> _users;

		public UserRepository(IStoreDatabaseSettings settings) {
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_users = database.GetCollection<User>("users");
		}

		public async Task<User> Create(User user) {
			user.Id = Guid.NewGuid().ToString("N");
			await _users.InsertOneAsync(user);
			return user;
		}

		public async Task Delete(string id) => await _users.DeleteOneAsync(x => x.Id == id);

		public async Task<User> Get(string id) => (await _users.FindAsync(user => user.Id == id).ConfigureAwait(false)).FirstOrDefault();

		public async Task<IEnumerable<User>> Get() => (await _users.FindAsync(user => true).ConfigureAwait(false)).ToList();

        public async Task<User> Get(string userName, string password) => (await _users.FindAsync(user => user.UserName == userName && user.Password == password).ConfigureAwait(false)).FirstOrDefault();

        public async Task Update(string id, User user) => await _users.ReplaceOneAsync(user => user.Id == id, user);

	}
}
