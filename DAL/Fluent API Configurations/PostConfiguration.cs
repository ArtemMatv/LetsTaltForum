using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Fluent_API_Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(p => p.Message)
                .IsRequired();

            builder.Property(p => p.DateCreated)
                .IsRequired();

            builder.HasOne(p => p.Topic)
                .WithMany(t=>t.Posts);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts);

            builder.HasMany(p => p.Comments)
                .WithOne(c=>c.Post);

            //builder.Property(p => p.UserUserName).HasConversion<User>();
        }
    }
}
