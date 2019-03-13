using ITNews.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.EntityConfig
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PostName).HasMaxLength(140).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Content).HasMaxLength(5000).IsRequired();
            builder.Property(p => p.CreatedById).HasMaxLength(450).IsRequired();

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
