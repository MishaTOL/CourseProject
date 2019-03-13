using ITNews.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITNews.Data.EntityConfig
{
    public class PhotoConfig : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FileName).HasMaxLength(256).IsRequired();

            builder.HasOne((System.Linq.Expressions.Expression<Func<Photo, Post>>)(p => (Post)p.Post))
                .WithMany((Post p) => (IEnumerable<Photo>)p.Photos)
                .HasForeignKey(p => p.PostId);
        }
    }
}
