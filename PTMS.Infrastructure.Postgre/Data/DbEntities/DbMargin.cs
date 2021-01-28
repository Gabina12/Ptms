using System;
using System.ComponentModel.DataAnnotations;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbMargin
    {
        [Key]
        public Guid Id { get; set; }

        public string Top { get; set; }

        public string Bottom { get; set; }

        public string Right { get; set; }

        public string Left { get; set; }
    }
}
