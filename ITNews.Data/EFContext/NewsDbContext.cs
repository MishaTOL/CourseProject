using ITNews.Core.Domain;
using ITNews.Data.EntityConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITNews.Data.EFContext
{
    public class NewsDbContext : IdentityDbContext
    {
        public NewsDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Role"); });

            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });

            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });

            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });

            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });

            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new LikeConfig());
            builder.ApplyConfiguration(new PhotoConfig());
            builder.ApplyConfiguration(new TagConfig());
            builder.ApplyConfiguration(new PostRatingConfig());

            builder.Entity<PostTag>()
            .HasKey(t => new { t.PostId, t.TagId });

            builder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(pt => pt.PostTags)
                .HasForeignKey(p => p.PostId);

            builder.Entity<PostTag>()
                .HasOne(t => t.Tag)
                .WithMany(pt => pt.PostTags)
                .HasForeignKey(t => t.TagId);
        }
    }
}
