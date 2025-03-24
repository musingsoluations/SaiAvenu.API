using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SriSai.Domain.Entity.Collection;

namespace SriSai.infrastructure.Persistent.EntityConfiguration;

public class ExpenseEntityConfiguration : IEntityTypeConfiguration<ExpenseEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseEntity> builder)
    {
        builder.ToTable("Expense");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Amount)
            .HasPrecision(18, 2);

        builder.Property(e => e.Type)
            .HasConversion<string>();
    }
}
