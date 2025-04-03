using ErrorOr;
using MediatR;

namespace SriSai.Application.Building.Query
{
    public record GetAllApartmentsQuery() : IRequest<ErrorOr<List<ListApartmentsQueryData>>>;
}


public class ListApartmentsQueryData
{
    public required string ApartmentNumber { get; set; }
    public required string OwnerName { get; set; }
    public string? RenterName { get; set; }
    public required Guid OwnerId { get; set; }
    public Guid? RenterId { get; set; }
}