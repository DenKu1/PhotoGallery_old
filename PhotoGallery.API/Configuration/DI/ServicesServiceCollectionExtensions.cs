using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.BLL.Helpers;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.BLL.Services;

namespace PhotoGallery.API.Configuration.DI
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IServiceHelper, ServiceHelper>();

            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}