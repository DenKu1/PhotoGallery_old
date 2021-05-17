using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.DAL.Interfaces;
using PhotoGallery.DAL.Repositories;

namespace PhotoGallery.BLL.Configuration.DI
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
