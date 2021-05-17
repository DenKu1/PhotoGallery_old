using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PhotoGallery.DAL.Entities;

namespace PhotoGallery.DAL.EF.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role() { Id = -1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = -2, Name = "User", NormalizedName = "USER" }
            );
        }
    }
}