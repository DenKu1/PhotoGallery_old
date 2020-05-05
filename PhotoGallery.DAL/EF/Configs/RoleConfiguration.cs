using System.Collections.Generic;
using PhotoGallery.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhotoGallery.DAL.EF.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {            
            builder.HasData(new List<Role>
            {
                new Role { Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Name = "User", NormalizedName = "USER" }
            });
        }
    }
}