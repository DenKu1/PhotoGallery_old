using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using PhotoGallery.DAL.Entities;

namespace PhotoGallery.DAL.EF
{
    public class GalleryContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
        {
            Database.EnsureCreated(); //TODO: Check this
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(GalleryContext).Assembly);
        }
    }
}
