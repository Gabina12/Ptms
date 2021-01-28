using System;
using System.ComponentModel.DataAnnotations;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbPartial
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(250)]
        public string Rev { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public string TemplateBody { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(250)]
        public string CreatedBy { get; set; }

        public DateTime? Updated { get; set; }

        [MaxLength(250)]
        public string Editor { get; set; }

        [MaxLength(250)]
        public string Category { get; set; }

        [MaxLength(250)]
        public string Version { get; set; }
    }
}
