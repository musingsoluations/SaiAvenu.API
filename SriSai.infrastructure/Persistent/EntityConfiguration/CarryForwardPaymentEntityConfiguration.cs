using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Collection;
using System;

namespace SriSai.infrastructure.Persistent.EntityConfiguration;

public class CarryForwardPaymentEntityConfiguration : IEntityTypeConfiguration<CarryForwardPayment>
{
    public void Configure(EntityTypeBuilder<CarryForwardPayment> builder)
    {
        builder.ToTable("CarryForwardPayment");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).IsRequired().HasPrecision(8, 2);
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(100);
    }
}