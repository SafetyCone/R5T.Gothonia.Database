using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Siscia;
using R5T.Venetia;


namespace R5T.Gothonia.Database
{
    public class TextItemRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, ITextItemRepository
        where TDbContext: DbContext, ITextItemDbContext
    {
        public TextItemRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public async Task<TextItemIdentity> New()
        {
            var textItemIdentity = TextItemIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var textItemEntity = new Entities.TextItem()
                {
                    GUID = textItemIdentity.Value,
                };

                dbContext.Add(textItemEntity);

                await dbContext.SaveChangesAsync();
            });

            return textItemIdentity;
        }

        public async Task<TextItemIdentity> New(TextItemTypeIdentity typeIdentity, string value)
        {
            var textItemIdentity = TextItemIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var textItemTypeID = await dbContext.TextItemTypes.Where(x => x.GUID == typeIdentity.Value).Select(x => x.ID).SingleAsync();

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

        public async Task<TextItemIdentity> Add(TextItem textItem)
        {
            var textItemIdentity = TextItemIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var textItemTypeID = await dbContext.TextItemTypes.Where(x => x.GUID == textItem.Type.Identity.Value).Select(x => x.ID).SingleAsync();

                var entity = new Entities.TextItem()
                {
                    GUID = textItemIdentity.Value,
                    TextItemTypeID = textItemTypeID,
                    Value = textItem.Value,
                };

                dbContext.TextItems.Add(entity);

                await dbContext.SaveChangesAsync();
            });

            return textItemIdentity;
        }

        public async Task<bool> Exists(TextItemIdentity identity)
        {
            var exists = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItems.Where(x => x.GUID == identity.Value).SingleOrDefaultAsync();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public async Task SetItemType(TextItemIdentity identity, TextItemTypeIdentity typeIdentity)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItems.Where(x => x.GUID == identity.Value).SingleAsync();

                var textItemTypeEntityID = await dbContext.TextItemTypes.Where(x => x.GUID == typeIdentity.Value).Select(x => x.ID).SingleAsync();

                entity.TextItemTypeID = textItemTypeEntityID;

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<TextItemTypeIdentity> GetItemType(TextItemIdentity identity)
        {
            var textItemTypeIdentity = await this.ExecuteInContextAsync(async dbContext =>
            {
                var textItemTypeGuid = await dbContext.TextItems.Where(x => x.GUID == identity.Value).Select(x => x.TextItemType.GUID).SingleAsync();

                var output = TextItemTypeIdentity.From(textItemTypeGuid);
                return output;
            });

            return textItemTypeIdentity;
        }

        public async Task SetValue(TextItemIdentity identity, string value)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItems.Where(x => x.GUID == identity.Value).SingleAsync();

                entity.Value = value;

                dbContext.SaveChanges();
            });
        }

        public async Task<string> GetValue(TextItemIdentity identity)
        {
            var value = await this.ExecuteInContextAsync(async dbContext =>
            {
                var output = await dbContext.TextItems.Where(x => x.GUID == identity.Value).Select(x => x.Value).SingleAsync();
                return output;
            });

            return value;
        }

        public async Task Delete(TextItemIdentity identity)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItems.Where(x => x.GUID == identity.Value).SingleAsync();

                dbContext.Remove(entity);

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<List<(string TypeName, string Value)>> GetTypedValuePairs(IEnumerable<TextItemIdentity> textItemIdentities)
        {
            var textItemIdentityValues = textItemIdentities.Select(x => x.Value);

            var anonymousValues = await this.ExecuteInContext(async dbContext =>
            {
                var values = await dbContext.TextItems
                    .Include(x => x.TextItemType)
                    .Where(x => textItemIdentityValues.Contains(x.GUID))
                    .Select(x => new { TypeName = x.TextItemType.Name, x.Value }) // Named tuples cannot appear in an expression tree, so use anonymous types and convert.
                    .ToListAsync();

                return values;
            });

            var typedValues = anonymousValues
                .Select(x => (TypeName: x.TypeName, Value: x.Value))
                .ToList();

            return typedValues;
        }
    }
}
