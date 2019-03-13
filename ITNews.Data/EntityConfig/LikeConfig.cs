using ITNews.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.EntityConfig
{
    public class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Like");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.LikeById).HasMaxLength(450).IsRequired();

            builder.HasOne((System.Linq.Expressions.Expression<Func<Like, Comment>>)(l => (Comment)l.Comment))
                .WithMany((Comment u) => (IEnumerable<Like>)u.Likes)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.LikeBy)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.LikeById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
