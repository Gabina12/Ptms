using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbVersion
    {
        [Key]
        public Guid Id { get; set; }
        public string VersionNumber { get; set; }

        public string Creator { get; set; }

        public DateTime Created { get; set; }

        public string TemplateBody { get; set; }

        public Guid TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public virtual DbTemplate Template { get; set; }
    }
}
