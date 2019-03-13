using ITNews.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.EntityConfig
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CommentById).HasMaxLength(450).IsRequired();
            builder.Property(c => c.Content).IsRequired();

            builder.HasOne((System.Linq.Expressions.Expression<Func<Comment, Post>>)(c => (Post)c.Post))
                .WithMany((Post p) => (IEnumerable<Comment>)p.Comments)
                .HasForeignKey(c => c.PostId);

            builder.HasOne(c => c.CommentBy)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CommentById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
