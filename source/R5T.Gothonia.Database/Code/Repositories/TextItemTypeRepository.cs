using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using R5T.Siscia;
using R5T.Venetia;


namespace R5T.Gothonia.Database
{
    public class TextItemTypeRepository : DatabaseRepositoryBase<TextItemDbContext>, ITextItemTypeRepository
    {
        public TextItemTypeRepository(DbContextOptions<TextItemDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public override TextItemDbContext GetNewDbContext()
        {
            var dbContext = new TextItemDbContext(this.DbContextOptions);
            return dbContext;
        }

        public TextItemTypeIdentity New()
        {
            var textItemTypeIdentity = TextItemTypeIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemTypeIdentity.Value,
                };

                dbContext.Add(entity);

                dbContext.SaveChanges();
            });

            return textItemTypeIdentity;
        }

        public TextItemTypeIdentity New(string name)
        {
            var textItemTypeIdentity = TextItemTypeIdentity.New();

            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemTypeIdentity.Value,
                    Name = name,
                };

                dbContext.Add(entity);

                dbContext.SaveChanges();
            });

            return textItemTypeIdentity;
        }

        public void Add(TextItemType textItemType)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemType.Identity.Value,
                    Name = textItemType.Name,
                };

                dbContext.Add(entity);

                dbContext.SaveChanges();
            });
        }

        public bool Exists(TextItemTypeIdentity identity)
        {
            var exists = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).SingleOrDefault();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public TextItemType Get(TextItemTypeIdentity identity)
        {
            var textItemType = this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).Single();

                var output = new TextItemType()
                {
                    Identity = TextItemTypeIdentity.From(entity.GUID),
                    Name = entity.Name,
                };
                return output;
            });

            return textItemType;
        }

        public string GetName(TextItemTypeIdentity identity)
        {
            var name = this.ExecuteInContext(dbContext =>
            {
                var output = dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).Select(x => x.Name).Single();
                return output;
            });

            return name;
        }

        public void SetName(TextItemTypeIdentity identity, string name)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).Single();

                entity.Name = name;

                dbContext.SaveChanges();
            });
        }

        public void Delete(TextItemTypeIdentity identity)
        {
            this.ExecuteInContext(dbContext =>
            {
                var entity = dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).Single();

                dbContext.TextItemTypes.Remove(entity);

                dbContext.SaveChanges();
            });
        }
    }
}
