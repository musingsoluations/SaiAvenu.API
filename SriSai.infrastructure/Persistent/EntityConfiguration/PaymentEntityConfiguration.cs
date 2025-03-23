using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Collection;

namespace SriSai.infrastructure.Persistent.EntityConfiguration
{
    public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount).IsRequired().HasPrecision(8, 2);
            builder.Property(x => x.PaidDate).IsRequired();
            builder.HasOne(x => x.FeeCollection)
                .WithMany(f => f.Payments) // ✅ Make it bidirectional
                .HasForeignKey(k => k.FeeCollectionId) // ✅ Correct FK mapping
                .IsRequired();
        }
    }
}