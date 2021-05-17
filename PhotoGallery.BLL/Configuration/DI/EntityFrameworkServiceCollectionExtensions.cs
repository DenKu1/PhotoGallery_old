using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;

namespace PhotoGallery.BLL.Configuration.DI
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<GalleryContext>();

            services.AddDbContext<GalleryContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(connectionString, b => b.MigrationsAssembly("PhotoGallery.DAL")));

            return services;
        }
    }
}
