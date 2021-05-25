using System;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.API;
using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;

namespace PhotoGallery.IntegrationTests.Utilities
{
    public class PhotoGalleryWebApplicationFactory : WebApplicationFactory<Startup> //<NoAuthStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(descriptor => descriptor
                    .ServiceType == typeof(DbContextOptions<GalleryContext>)));

                var serviceProvider = GetInMemoryServiceProvider();

                services.AddDbContextPool<GalleryContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.Empty.ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<GalleryContext>();

                SeedData(context);
            });
        }

        private void SeedData(GalleryContext context)
        {
            var userId = TestUser.Id;

            var album1 = new Album { Name = "album1", Description = "desc1", Created = DateTime.Now.AddDays(-1), Updated = DateTime.Now, UserId = userId, };
            var album2 = new Album { Name = "album2", Description = "desc2", Created = DateTime.Now.AddDays(-1), Updated = DateTime.Now, UserId = userId, };

            var photo1 = new Photo { Name = "photo1", Path = "path", Created = DateTime.Now, Album = album1 };
            var photo2 = new Photo { Name = "photo2", Path = "path", Created = DateTime.Now, Album = album1 };
            var photo3 = new Photo { Name = "photo3", Path = "path", Created = DateTime.Now, Album = album2 };

            var comment1 = new Comment { Text = "comment1", UserId = userId, Photo = photo1 };
            var comment2 = new Comment { Text = "comment2", UserId = userId, Photo = photo1 };
            var comment3 = new Comment { Text = "comment3", UserId = userId, Photo = photo1 };

            context.AddRange(new Album[] { album1, album2 });
            context.AddRange(new Photo[] { photo1, photo2, photo3 });
            context.AddRange(new Comment[] { comment1, comment2, comment3 });

            context.SaveChanges();
        }

        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }
    }
}
