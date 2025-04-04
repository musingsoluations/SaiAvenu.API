using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Building;

namespace SriSai.infrastructure.Persistent.EntityConfiguration
{
    public class ApartmentEntityConfiguration : IEntityTypeConfiguration<ApartmentEntity>
    {
        public void Configure(EntityTypeBuilder<ApartmentEntity> builder)
        {
            builder.ToTable("Apartment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ApartmentNumber).IsRequired().HasMaxLength(5);

            // Required Owner Relationship (One-to-Many)
            builder.HasOne(a => a.Owner)
                .WithMany(u => u.OwnedApartments)
                .HasForeignKey(a => a.OwnerId)
                .IsRequired();
            builder.HasOne(a => a.Renter)
                .WithMany(u => u.RentedApartments)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(a => a.RenterId);
        }
    }
}