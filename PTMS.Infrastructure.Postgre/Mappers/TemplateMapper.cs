using System;
using System.Linq;

namespace PTMS.Infrastructure.Postgre.Mappers
{
    public static class TemplateMapper
    {
        public static Core.Models.Template AsDomain(this Data.DbEntities.DbTemplate template) => new Core.Models.Template
        {
            Caching = template.Caching,
            Category = template.Category,
            Created = (long)template.Created.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
            Creator = template.Creator,
            Description = template.Description,
            Editor = template.Editor,
            Id = template.Id.ToString(),
            LoadExternalSources = template.LoadExternalSources,
            Name = template.Name,
            Rev = template.Rev,
            TemplateBody = template.TemplateBody,
            Updated = (long)template.Updated?.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
            Version = template.Version,
            Options = template.Options != null ? new Core.Models.Options
            {
                DisplayHeaderFooter = template.Options.DisplayHeaderFooter,
                FooterTemplate = template.Options.FooterTemplate,
                Format = template.Options.Format,
                HeaderTemplate = template.Options.HeaderTemplate,
                Landscape = template.Options.Landscape,
                Scale = template.Options.Scale,
                PrintBackground = template.Options.PrintBackground,
                Margin = new Core.Models.Margin
                {
                    Bottom = template.Options.Margin.Bottom,
                    Left = template.Options.Margin.Left,
                    Right = template.Options.Margin.Right,
                    Top = template.Options.Margin.Top
                }
            } : null,
            Versions = template.Versions?.Select(x=> new Core.Models.Version
            {
                Created = x.Created,
                Creator = x.Creator,
                TemplateBody = x.TemplateBody,
                VersionNumber = x.VersionNumber
            }).ToArray()
        };

        public static Data.DbEntities.DbTemplate AsEntity(this Core.Models.Template template) => new Data.DbEntities.DbTemplate
        {
            Id = Guid.Parse(template.Id),
            Caching = template.Caching,
            Category = template.Category,
            Created = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(template.Created).ToLocalTime(),
            Creator = template.Creator,
            Description = template.Description,
            Editor = template.Editor,
            LoadExternalSources = template.LoadExternalSources,
            Name = template.Name,
            Rev = template.Rev,
            TemplateBody = template.TemplateBody,
            Updated = template.Updated > 0 ? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(template.Updated).ToLocalTime() : (DateTime?)null,
            Version = template.Version,
            Options = new Data.DbEntities.DbOption
            {
                DisplayHeaderFooter = template.Options.DisplayHeaderFooter,
                FooterTemplate = template.Options.FooterTemplate,
                Format = template.Options.Format,
                HeaderTemplate = template.Options.HeaderTemplate,
                Landscape = template.Options.Landscape,
                Scale = template.Options.Scale,
                PrintBackground = template.Options.PrintBackground,
                Margin = new Data.DbEntities.DbMargin
                {
                    Bottom = template.Options.Margin.Bottom,
                    Left = template.Options.Margin.Left,
                    Right = template.Options.Margin.Right,
                    Top = template.Options.Margin.Top
                }
            },
            Versions = template.Versions?.Select(x => new Data.DbEntities.DbVersion
            {
                Created = x.Created,
                Creator = x.Creator,
                TemplateBody = x.TemplateBody,
                VersionNumber = x.VersionNumber
            }).ToArray()
        };
    }
}
