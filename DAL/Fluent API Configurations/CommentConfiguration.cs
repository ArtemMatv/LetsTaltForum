using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Fluent_API_Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DateCreated)
                .IsRequired();

            builder.Property(c => c.Message)
                .IsRequired();

            builder.HasOne(c=>c.User)
                .WithMany(u=>u.Comments);

            builder.HasOne(c => c.Post)
                .WithMany(p=>p.Comments);
        }
    }
}
