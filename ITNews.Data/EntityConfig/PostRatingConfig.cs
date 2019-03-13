using ITNews.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.EntityConfig
{
    public class PostRatingConfig : IEntityTypeConfiguration<PostRating>
    {
        public void Configure(EntityTypeBuilder<PostRating> builder)
        {
            builder.ToTable("PostRating");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.UserId).HasMaxLength(450).IsRequired();
            builder.Property(r => r.Rating);

            builder.HasOne(r => r.Post)
                .WithMany(p => p.Ratings)
                .HasForeignKey(c => c.PostId);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
