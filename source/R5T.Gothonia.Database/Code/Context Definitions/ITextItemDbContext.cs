using System;

using Microsoft.EntityFrameworkCore;


namespace R5T.Gothonia.Database
{
    public interface ITextItemDbContext
    {
        DbSet<Entities.TextItem> TextItems { get; set; }
        DbSet<Entities.TextItemType> TextItemTypes { get; set; }
    }
}
