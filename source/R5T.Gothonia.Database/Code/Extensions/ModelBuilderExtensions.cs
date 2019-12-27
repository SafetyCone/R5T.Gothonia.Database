using System;

using Microsoft.EntityFrameworkCore;


namespace R5T.Gothonia.Database
{
    public static class ModelBuilderExtensions
    {
        public static void ForTextItemDbContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.TextItem>().HasAlternateKey(x => x.GUID);
            modelBuilder.Entity<Entities.TextItemType>().HasAlternateKey(x => x.GUID);
            modelBuilder.Entity<Entities.TextItemType>().HasAlternateKey(x => x.Name);
        }
    }
}
