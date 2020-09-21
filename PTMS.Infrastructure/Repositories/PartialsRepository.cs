using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PTMS.Core.Models;
using PTMS.Core.Repositories;

namespace PTMS.Infrastructure {
    public class PartialsRepository : IPartialsRepository {
		private readonly IMongoCollection<PartialTemplate> _users;

		public PartialsRepository(IStoreDatabaseSettings settings) {
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_users = database.GetCollection<PartialTemplate>("partials");
		}

		public async Task<PartialTemplate> Create(PartialTemplate template) {
			await _users.InsertOneAsync(template);
			return template;
		}

		public async Task Delete(string id) => await _users.DeleteOneAsync(x => x.Id == id);

		public async Task<PartialTemplate> Get(string id) => (await _users.FindAsync(template => template.Id == id).ConfigureAwait(false)).FirstOrDefault();

		public async Task<IEnumerable<PartialTemplate>> Get() => (await _users.FindAsync(template => true).ConfigureAwait(false)).ToList();

		public async Task Update(string id, PartialTemplate template) => await _users.ReplaceOneAsync(template => template.Id == id, template);
	}
}
