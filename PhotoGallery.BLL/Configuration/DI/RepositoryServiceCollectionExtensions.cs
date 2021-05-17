using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.DAL;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.DAL.Repositories;

namespace PhotoGallery.BLL.Configuration.DI
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
