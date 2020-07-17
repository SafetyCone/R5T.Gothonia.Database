using System;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using R5T.Dacia;


namespace R5T.Gothonia.Database
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="TextItemRepository{TDbContext}"/> implementation of <see cref="ITextItemRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddTextItemRepository<TDbContext>(this IServiceCollection services)
            where TDbContext: DbContext, ITextItemDbContext
        {
            services.AddSingleton<ITextItemRepository, TextItemRepository<TDbContext>>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="TextItemRepository{TDbContext}"/> implementation of <see cref="ITextItemRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ITextItemRepository> AddTextItemRepositoryAction<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITextItemDbContext
        {
            var serviceAction = ServiceAction.New<ITextItemRepository>(() => services.AddTextItemRepository<TDbContext>());
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="TextItemTypeRepository{TDbContext}"/> implementation of <see cref="ITextItemTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddTextItemTypeRepository<TDbContext>(this IServiceCollection services)
            where TDbContext: DbContext, ITextItemDbContext
        {
            services.AddSingleton<ITextItemTypeRepository, TextItemTypeRepository<TDbContext>>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="TextItemTypeRepository{TDbContext}"/> implementation of <see cref="ITextItemTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ITextItemTypeRepository> AddTextItemTypeRepositoryAction<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, ITextItemDbContext
        {
            var serviceAction = ServiceAction.New<ITextItemTypeRepository>(() => services.AddTextItemTypeRepository<TDbContext>());
            return serviceAction;
        }
    }
}
