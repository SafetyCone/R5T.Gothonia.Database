using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using R5T.Siscia;
using R5T.Venetia;


namespace R5T.Gothonia.Database
{
    public class TextItemRepository : DatabaseRepositoryBase<TextItemDbContext>, ITextItemRepository
    {
        public TextItemRepository(DbContextOptions<TextItemDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public override TextItemDbContext GetNewDbContext()
        {
            var dbContext = new TextItemDbContext(this.DbContextOptions);
            return dbContext;
        }

        public TextItemIdentity New()
        {
            var textItemIdentity = TextItemIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.TextItem()
                {
                    GUID = textItemIdentity.Value,
                };
                return entity;
            });

            return textItemIdentity;
        }

        public TextItemIdentity New(TextItemTypeIdentity typeIdentity, string value)
        {
            var textItemIdentity = TextItemIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var textItemTypeID = dbContext.TextItemTypes.Where(x => x.GUID == typeIdentity.Value).Select(x => x.ID).Single();

                var entity = new Entities.TextItem()
                {
                    GUID = textItemIdentity.Value,
                    TextItemTypeID = textItemTypeID,
                    Value = value,
                };
                return entity;
            });

            return textItemIdentity;
        }

        public TextItemIdentity Add(TextItem textItem)
        {
            var textItemIdentity = TextItemIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var textItemTypeID = dbContext.TextItemTypes.Where(x => x.GUID == textItem.Type.Identity.Value).Select(x => x.ID).Single();

                var entity = new Entities.TextItem()
                {
                    GUID = textItem.Identity.Value,
                    TextItemTypeID = textItemTypeID,
                    Value = textItem.Value,
                };

                dbContext.TextItems.Add(entity);

                dbContext.SaveChanges();
            });

            return textItemIdentity;
        }

        public bool Exists(TextItemIdentity identity)
        {
            var exists = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItems.Where(x => x.GUID == identity.Value).SingleOrDefault();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public void SetItemType(TextItemIdentity identity, TextItemTypeIdentity typeIdentity)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItems.Where(x => x.GUID == identity.Value).Single();

                var textItemTypeEntityID = dbContext.TextItemTypes.Where(x => x.GUID == typeIdentity.Value).Select(x => x.ID).Single();

                entity.TextItemTypeID = textItemTypeEntityID;

                dbContext.SaveChanges();
            });
        }

        public TextItemTypeIdentity GetItemType(TextItemIdentity identity)
        {
            var textItemTypeIdentity = this.ExecuteInContext(dbContext =>
            {
                var textItemTypeGuid = dbContext.TextItems.Where(x => x.GUID == identity.Value).Select(x => x.TextItemType.GUID).Single();

                var output = TextItemTypeIdentity.From(textItemTypeGuid);
                return output;
            });

            return textItemTypeIdentity;
        }

        public void SetValue(TextItemIdentity identity, string value)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItems.Where(x => x.GUID == identity.Value).Single();

                entity.Value = value;

                dbContext.SaveChanges();
            });
        }

        public string GetValue(TextItemIdentity identity)
        {
            var value = this.ExecuteInContext(dbContext =>
            {
                var output = dbContext.TextItems.Where(x => x.GUID == identity.Value).Select(x => x.Value).Single();
                return output;
            });

            return value;
        }

        public void Delete(TextItemIdentity identity)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItems.Where(x => x.GUID == identity.Value).Single();

                dbContext.TextItems.Remove(entity);
                return dbContext;
            });
        }
    }
}
