using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbTemplate
    {
        public Guid Id { get; set; }

        public string Rev { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long Created { get; set; }

        public string Creator { get; set; }

        public long Updated { get; set; }

        public string Editor { get; set; }

        public string TemplateBody { get; set; }

        public bool Caching { get; set; }

        public bool LoadExternalSources { get; set; }

        public string Category { get; set; }

        public string Version { get; set; }

        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public virtual DbOption Options { get; set; }

        public virtual ICollection<DbVersion> Versions { get; set; }
    }
}
