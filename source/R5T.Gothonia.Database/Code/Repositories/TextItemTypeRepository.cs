using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using R5T.Siscia;
using R5T.Venetia;


namespace R5T.Gothonia.Database
{
    public class TextItemTypeRepository<TDbContext> : ProvidedDatabaseRepositoryBase<TDbContext>, ITextItemTypeRepository
        where TDbContext: DbContext, ITextItemDbContext
    {
        public TextItemTypeRepository(DbContextOptions<TDbContext> dbContextOptions, IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextOptions, dbContextProvider)
        {
        }

        public async Task<TextItemTypeIdentity> New()
        {
            var textItemTypeIdentity = TextItemTypeIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemTypeIdentity.Value,
                };

                dbContext.Add(entity);

                await dbContext.SaveChangesAsync();
            });

            return textItemTypeIdentity;
        }

        public async Task<TextItemTypeIdentity> New(string name)
        {
            var textItemTypeIdentity = TextItemTypeIdentity.New();

            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemTypeIdentity.Value,
                    Name = name,
                };

                dbContext.Add(entity);

                await dbContext.SaveChangesAsync();
            });

            return textItemTypeIdentity;
        }

        public async Task Add(TextItemType textItemType)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = new Entities.TextItemType()
                {
                    GUID = textItemType.Identity.Value,
                    Name = textItemType.Name,
                };

                dbContext.Add(entity);

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<bool> Exists(string name)
        {
            var exists = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.Name == name).SingleOrDefaultAsync();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public async Task<bool> Exists(TextItemTypeIdentity identity)
        {
            var exists = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).SingleOrDefaultAsync();

                var output = entity is object;
                return output;
            });

            return exists;
        }

        public async Task<TextItemTypeIdentity> GetIdentity(string name)
        {
            var identity = await this.ExecuteInContextAsync(async dbContext =>
            {
                var guid = await dbContext.TextItemTypes.Where(x => x.Name == name).Select(x => x.GUID).SingleAsync();

                var output = TextItemTypeIdentity.From(guid);
                return output;
            });

            return identity;
        }

        public async Task<TextItemType> Get(string name)
        {
            var textItemType = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.Name == name).SingleAsync();

                var output = new TextItemType()
                {
                    Identity = TextItemTypeIdentity.From(entity.GUID),
                    Name = entity.Name, // Use entity name, not the input name for exactness.
                };
                return output;
            });

            return textItemType;
        }

        public async Task<TextItemType> Get(TextItemTypeIdentity identity)
        {
            var textItemType = await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).SingleAsync();

                var output = new TextItemType()
                {
                    Identity = TextItemTypeIdentity.From(entity.GUID),
                    Name = entity.Name,
                };
                return output;
            });

            return textItemType;
        }

        public async Task<string> GetName(TextItemTypeIdentity identity)
        {
            var name = await this.ExecuteInContextAsync(async dbContext =>
            {
                var output = await dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).Select(x => x.Name).SingleAsync();
                return output;
            });

            return name;
        }

        public async Task SetName(TextItemTypeIdentity identity, string name)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).SingleAsync();

                entity.Name = name;

                dbContext.SaveChanges();
            });
        }

        public async Task Delete(TextItemTypeIdentity identity)
        {
            await this.ExecuteInContextAsync(async dbContext =>
            {
                var entity = await dbContext.TextItemTypes.Where(x => x.GUID == identity.Value).SingleAsync();

                dbContext.TextItemTypes.Remove(entity);

                await dbContext.SaveChangesAsync();
            });
        }
    }
}
