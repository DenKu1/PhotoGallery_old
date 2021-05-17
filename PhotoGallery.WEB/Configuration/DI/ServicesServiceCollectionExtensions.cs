using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.BLL.Interfaces;
using PhotoGallery.BLL.Services;

namespace PhotoGallery.WEB.Configuration.DI
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}