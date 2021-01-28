using Microsoft.EntityFrameworkCore;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Infrastructure.Postgre.Data;
using PTMS.Infrastructure.Postgre.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTMS.Infrastructure.Postgre
{
    public class PartialsPostgreRepository : IPartialsRepository
    {
        private readonly PtmsDataStore _store;

        public PartialsPostgreRepository(PtmsDataStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<PartialTemplate> Create(PartialTemplate template)
        {
            var dbPartial = template.AsEntity();
            await _store.Partials.AddAsync(dbPartial).ConfigureAwait(false);
            await _store.SaveChangesAsync().ConfigureAwait(false);
            return dbPartial.AsDomain();
        }

        public async Task Delete(string id)
        {
            var dbPartial = await _store.Partials.FindAsync(Guid.Parse(id)).ConfigureAwait(false);
            _store.Partials.Remove(dbPartial);
            await _store.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<PartialTemplate> Get(string id) => (await _store.Partials.FindAsync(Guid.Parse(id)).ConfigureAwait(false))?.AsDomain();

        public async Task<IEnumerable<PartialTemplate>> Get() => (await _store.Partials.AsNoTracking().ToListAsync()).Select(x => x.AsDomain());

        public async Task Update(string id, PartialTemplate template)
        {
            var dbPartial = await _store.Partials.FindAsync(Guid.Parse(id)).ConfigureAwait(false);
            var dbPartialNew = template.AsEntity();
            _store.Partials.Attach(dbPartial);
            _store.Entry(dbPartial).CurrentValues.SetValues(dbPartialNew);
            await _store.SaveChangesAsync();
        }
    }
}
