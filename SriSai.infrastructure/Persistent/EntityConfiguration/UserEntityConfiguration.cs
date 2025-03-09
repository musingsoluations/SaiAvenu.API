using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Users;

namespace SriSai.infrastructure.Persistent.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Mobile).IsRequired().HasMaxLength(10);
            builder.HasIndex(u => u.Mobile).IsUnique();
            builder.Property(x => x.IsUserActive).IsRequired().HasColumnType("boolean");
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(38);
            builder.Property(x => x.CreatedDateTime).IsRequired().HasColumnType("timestamp");
            builder.Property(x => x.IsDeleted).IsRequired().HasColumnType("boolean");
            builder.Property(x => x.DeletedBy).IsRequired().HasMaxLength(38);
            builder.Property(x => x.DeletedDateTime).IsRequired().HasColumnType("timestamp");
            builder.Property(x => x.UpdatedDateTime).IsRequired().HasColumnType("timestamp");
            builder.Property(x => x.UpdatedById).IsRequired().HasMaxLength(38);
            builder.HasMany(x => x.Roles);
        }
    }

    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserRoleName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.IsDeleted).IsRequired().HasColumnType("boolean");
        }
    }
}