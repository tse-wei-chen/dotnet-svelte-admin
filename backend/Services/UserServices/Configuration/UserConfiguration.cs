using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Services.UserServices.Model;

namespace backend.Services.UserServices.Configuration
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("users");
			builder.HasKey(u => u.Id);
			builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);
			builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
			builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
			builder.Property(u => u.CreatedAt).IsRequired();
			builder.Property(u => u.RefreshToken);
			builder.Property(u => u.RefreshTokenExpiryTime);
			builder.HasIndex(u => u.Email).IsUnique();
		}
	}
}
