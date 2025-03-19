using SriSai.Domain.Entity.Base;
using SriSai.Domain.Entity.Building;

namespace SriSai.Domain.Entity.Collection
{
    public class FeeCollectionEntity : EntityBase
    {
        public Guid ApartmentId { get; set; }
        public ApartmentEntity Apartment { get; set; }
        public Guid FeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestForDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public bool IsPaid { get; set; }
        public CollectionType ForWhat { get; set; }
        public string? Comment { get; set; }
    }

    public enum CollectionType
    {
        MonthlyMaintenance,
        AdhocExpense
    }
}