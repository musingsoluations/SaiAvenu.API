using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Collection;

namespace SriSai.infrastructure.Persistent.EntityConfiguration
{
    public class FeeCollectionEntityConfiguration : IEntityTypeConfiguration<FeeCollectionEntity>
    {
        public void Configure(EntityTypeBuilder<FeeCollectionEntity> builder)
        {
            builder.ToTable("FeeCollection");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ForWhat).IsRequired();
            builder.Property(x => x.Amount).IsRequired().HasPrecision(8, 2);
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.IsPaid).IsRequired();
            builder.Property(x => x.RequestForDate).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(100);
            builder.HasOne(x => x.Apartment)
                .WithMany(a => a.FeeCollections) // ✅ Make it bidirectional
                .HasForeignKey(k => k.ApartmentId) // ✅ Correct FK mapping
                .IsRequired();
            ;
        }
    }
}