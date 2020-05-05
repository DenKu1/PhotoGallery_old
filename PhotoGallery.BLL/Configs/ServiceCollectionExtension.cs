using AutoMapper;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.BLL.Services;
using PhotoGallery.DAL;
using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PhotoGallery.BLL.Configs
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Bind(this IServiceCollection services, string connectionString)
        {
            var mapperConfig = new MapperConfiguration(c => c.AddProfile(new MapperProfile()));

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<GalleryContext>();

            services.AddDbContext<GalleryContext>(options =>
                    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("PhotoGallery.DAL")));

            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
