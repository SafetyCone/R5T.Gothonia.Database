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
    }
}
