using ErrorOr;
using MediatR;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Command
{
    public class CreateCollectionDemandCommand : IRequest<ErrorOr<IList<Guid>>>
    {
        public required string[] ApartmentName { get; set; }
        public decimal Amount { get; set; }
        public DateOnly RequestForDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? PaidDate { get; set; }
        public CollectionType ForWhat { get; set; }
        public string? Comment { get; set; }
        public Guid CreatedBy { get; set; }
    }
}