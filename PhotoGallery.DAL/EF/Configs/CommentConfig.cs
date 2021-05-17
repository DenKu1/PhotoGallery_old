using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PhotoGallery.DAL.Entities;

namespace PhotoGallery.DAL.EF.Configs
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(u => u.Text)
                .IsRequired()
                .HasMaxLength(300);
        }
    }
}
