using System;

using Microsoft.EntityFrameworkCore;


namespace R5T.Gothonia.Database
{
    public class TextItemDbContext : DbContext
    {
        public DbSet<Entities.TextItem> TextItems { get; set; }
        public DbSet<Entities.TextItemType> TextItemTypes { get; set; }


        public TextItemDbContext(DbContextOptions<TextItemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.TextItem>().HasAlternateKey(x => x.GUID);
            modelBuilder.Entity<Entities.TextItemType>().HasAlternateKey(x => x.GUID);
            modelBuilder.Entity<Entities.TextItemType>().HasAlternateKey(x => x.Name);
        }
    }
}
