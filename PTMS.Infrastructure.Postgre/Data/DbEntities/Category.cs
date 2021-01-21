using System;
using System.ComponentModel.DataAnnotations;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbCategory
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
