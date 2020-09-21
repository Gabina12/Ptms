using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PTMS.Core.Models;
using PTMS.Core.Repositories;

namespace PTMS.Infrastructure
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly IMongoCollection<Category> _categorys;

		public CategoryRepository(IStoreDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			_categorys = database.GetCollection<Category>("Categorys");
		}

		public async Task<Category> Create(Category Category)
		{
			await _categorys.InsertOneAsync(Category);
			return Category;
		}

		public async Task Delete(Guid id) => await _categorys.DeleteOneAsync(x => x.Id == id);

		public async Task<bool> Exists(string name) => (await _categorys.CountDocumentsAsync(x => x.Name == name)) > 0;

		public async Task<Category> Get(Guid id) => (await _categorys.FindAsync(Category => Category.Id == id).ConfigureAwait(false)).FirstOrDefault();

		public async Task<IEnumerable<Category>> Get() => (await _categorys.FindAsync(Category => true).ConfigureAwait(false)).ToList();

		public async Task Update(Guid id, Category Category) => await _categorys.ReplaceOneAsync(Category => Category.Id == id, Category);

	}
}
