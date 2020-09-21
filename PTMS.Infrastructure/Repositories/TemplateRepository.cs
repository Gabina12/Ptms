using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PTMS.Core.Models;
using PTMS.Core.Repositories;

namespace PTMS.Infrastructure {
    public class TemplateRepository : ITemplateRepository
	{
		private readonly IMongoCollection<Template> _users;

		public TemplateRepository(IStoreDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_users = database.GetCollection<Template>("templates");
		}

		public async Task<Template> Create(Template template)
		{
			await _users.InsertOneAsync(template);
			return template;
		}

		public async Task Delete(string id) => await _users.DeleteOneAsync(x => x.Id == id);

		public async Task<Template> Get(string id) => (await _users.FindAsync(template => template.Id == id).ConfigureAwait(false)).FirstOrDefault();

		public async Task<IEnumerable<Template>> Get() => (await _users.FindAsync(template => true).ConfigureAwait(false)).ToList();

		public async Task Update(string id, Template template) => await _users.ReplaceOneAsync(template => template.Id == id, template);

	}
}
