using Microsoft.EntityFrameworkCore;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Infrastructure.Postgre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PTMS.Infrastructure.Postgre.Mappers.TemplateMapper;

namespace PTMS.Infrastructure.Postgre
{
    public class TemplatePostgreRepository : ITemplateRepository
    {
        private readonly Data.PtmsDataStore _store;

        public TemplatePostgreRepository(PtmsDataStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<Template> Create(Template template)
        {
            var dbTemplate = template.AsEntity();
            await _store.AddAsync(dbTemplate).ConfigureAwait(false);
            await _store.SaveChangesAsync();
            return dbTemplate.AsDomain();
        }

        public async Task Delete(string id)
        {
            var dbTemplate = await _store.Templates
                                    .Include(x => x.Versions)
                                    .Include(x => x.Options)
                                    .ThenInclude(x => x.Margin)
                .FirstOrDefaultAsync(x=>x.Id == Guid.Parse(id)).ConfigureAwait(false);
            
            _store.Remove(dbTemplate);
            await _store.SaveChangesAsync();
        }

        public async Task<Template> Get(string id) {
            var template = await _store.Templates
                                    .Include(x => x.Versions)
                                    .Include(x => x.Options)
                                    .ThenInclude(x => x.Margin)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id))
                                    .ConfigureAwait(false);
            return template.AsDomain();
        }

        public async Task<IEnumerable<Template>> Get() {
            var templates = await _store.Templates.AsNoTracking().ToListAsync().ConfigureAwait(false);
            return templates.Select(x => x.AsDomain());
        }

        public async Task Update(string id, Template template)
        {
            await Delete(id);
            template.Id = id;
            await Create(template);
        }
    }
}
