using Microsoft.EntityFrameworkCore;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Infrastructure.Postgre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PTMS.Infrastructure.Postgre.Mappers.CategoryMapper;

namespace PTMS.Infrastructure.Postgre
{
    public class CategoryPostgreRepository : ICategoryRepository
    {
        private readonly PtmsDataStore _store;

        public CategoryPostgreRepository(PtmsDataStore store)
        {
            _store = store;
        }

        public async Task<Category> Create(Category template)
        {
            var dbCategory = template.AsEntity();
            await _store.Categories.AddAsync(dbCategory);
            await _store.SaveChangesAsync().ConfigureAwait(false);
            return dbCategory.AsDomain();
        }

        public async Task Delete(Guid id)
        {
            var dbCategory = await _store.Categories.FindAsync(id);
            _store.Categories.Remove(dbCategory);
            await _store.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> Exists(string name) => await _store.Categories.AsNoTracking().AnyAsync(x => x.Name == name).ConfigureAwait(false);

        public async Task<Category> Get(Guid id) => (await _store.Categories.FindAsync(id).ConfigureAwait(false))?.AsDomain();

        public async Task<IEnumerable<Category>> Get() => (await _store.Categories.AsNoTracking().ToListAsync()).Select(x => x.AsDomain());

        public async Task Update(Guid id, Category template)
        {
            var dbCategory = await _store.Categories.FindAsync(id).ConfigureAwait(false);
            dbCategory.Name = template.Name;
            dbCategory.Description = template.Description;
            await _store.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
