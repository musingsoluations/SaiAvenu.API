using SriSai.Domain.Entity.Collection;

namespace SriSai.API.DTOs.Collection
{
    public class CreateCollectionDemandDto
    {
        public required string[] ApartmentName { get; set; }
        public decimal Amount { get; set; }
        public DateOnly RequestForDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? PaidDate { get; set; }
        public CollectionType ForWhat { get; set; } // Assuming CollectionType is an enum, use underlying int type
        public string? Comment { get; set; }
    }
}