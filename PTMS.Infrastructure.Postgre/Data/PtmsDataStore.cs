using Microsoft.EntityFrameworkCore;
using PTMS.Infrastructure.Postgre.Data.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTMS.Infrastructure.Postgre.Data
{
    public class PtmsDataStore : DbContext
    {
        public PtmsDataStore(DbContextOptions<PtmsDataStore> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("ptms");
        }

        public DbSet<DbCategory> Categories { get; set; }
        public DbSet<DbTemplate> Templates { get; set; }

        public DbSet<DbPartial> Partials { get; set; }
        public DbSet<DbOption> Options { get; set; }
        public DbSet<DbMargin> Margins { get; set; }
    }
}
