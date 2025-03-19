using SriSai.Domain.Entity.Collection;

namespace SriSai.API.DTOs.Collection
{
    public class CreateCollectionDemandDto
    {
        public required string[] ApartmentName { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestForDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public bool IsPaid { get; set; }
        public CollectionType ForWhat { get; set; } // Assuming CollectionType is an enum, use underlying int type
        public string? Comment { get; set; }
    }
}