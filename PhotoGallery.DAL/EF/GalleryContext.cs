﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Tag> Tags { get; set; }

        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options)
        {
            //Turn on for integration tests; turn off for manual migrations
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(GalleryContext).Assembly);
        }
    }
}
