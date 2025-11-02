using AuthApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Data.Configurations;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
    }
}
