
using System;

namespace PTMS.Infrastructure.Postgre.Mappers
{
    public static class PartialMapper
    {
        public static Core.Models.PartialTemplate AsDomain(this Data.DbEntities.DbPartial partial) => new Core.Models.PartialTemplate
        {
            Id = partial.Id.ToString(),
            Category = partial.Category,
            Created = (long)partial.Created.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
            Creator = partial.CreatedBy,
            Description = partial.Description,
            Editor = partial.Editor,
            Name = partial.Name,
            TemplateBody = partial.TemplateBody,
            Updated = (long)partial.Updated?.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
            Version = partial.Version,
            Rev = partial.Rev
        };

        public static Data.DbEntities.DbPartial AsEntity(this Core.Models.PartialTemplate partial) => new Data.DbEntities.DbPartial
        {
            Id = Guid.Parse(partial.Id),
            Category = partial.Category,
            Created = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(partial.Created).ToLocalTime(),
            CreatedBy = partial.Creator,
            Description = partial.Description,
            Editor = partial.Editor,
            Name = partial.Name,
            Rev = partial.Rev,
            TemplateBody = partial.TemplateBody,
            Updated = partial.Updated > 0 ? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(partial.Updated).ToLocalTime() : (DateTime?)null,
            Version = partial.Version
        };
    }
}
