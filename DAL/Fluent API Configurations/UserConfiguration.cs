using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Fluent_API_Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasAlternateKey(u => u.UserName);

            builder.Property(u => u.Age);

            builder.Property(u => u.AvatarPath);

            builder.Property(u => u.Gender);

            builder.Property(u => u.BannedTo);

            builder.Property(u => u.SilencedTo);

            builder.HasMany(u => u.Comments)
                .WithOne(p => p.User);

            builder.HasMany(u => u.Posts)
               .WithOne(p => p.User);

            builder.Property(u => u.CapableToBan)
                .IsRequired();

            builder.Property(u => u.CapableToSilence)
                .IsRequired();

            builder.HasOne(u => u.UserRole);
        }
    }
}
