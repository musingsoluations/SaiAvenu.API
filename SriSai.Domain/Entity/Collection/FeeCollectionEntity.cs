using SriSai.Domain.Entity.Base;
using SriSai.Domain.Entity.Building;
using System.ComponentModel.DataAnnotations.Schema;

namespace SriSai.Domain.Entity.Collection
{
    public class FeeCollectionEntity : EntityBase
    {
        public Guid ApartmentId { get; set; }
        public ApartmentEntity Apartment { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestForDate { get; set; }
        public DateTime DueDate { get; set; }
        public CollectionType ForWhat { get; set; }
        public string? Comment { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        [NotMapped] public decimal RemainingAmount => Amount - (Payments?.Sum(p => p.Amount) ?? 0);
    }

    public enum CollectionType
    {
        MonthlyMaintenance,
        AdhocExpense
    }
}