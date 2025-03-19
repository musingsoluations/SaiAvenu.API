using ErrorOr;
using MediatR;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Command
{
    public class CreateCollectionDemandCommand : IRequest<ErrorOr<IList<Guid>>>
    {
        public required string[] ApartmentName { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestForDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public bool IsPaid { get; set; }
        public CollectionType ForWhat { get; set; }
        public string? Comment { get; set; }
    }
}