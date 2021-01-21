using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTMS.Infrastructure.Postgre.Data.DbEntities
{
    public class DbOption
    {
        [Key]
        public Guid Id { get; set; }

        public int Scale { get; set; }

        public bool DisplayHeaderFooter { get; set; }

        public string HeaderTemplate { get; set; }

        public string FooterTemplate { get; set; }

        public bool PrintBackground { get; set; }

        public bool Landscape { get; set; }

        public string Format { get; set; }

        public Guid MarginId { get; set; }

        [ForeignKey("MarginId")]
        public virtual DbMargin Margin { get; set; }
    }
}
