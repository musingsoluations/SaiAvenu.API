using ErrorOr;
using MediatR;

namespace SriSai.Application.Building.Command;

public record CreateApartmentCommand(
    string ApartmentNumber,
    Guid OwnerId,
    Guid? RenterId
) : IRequest<ErrorOr<Guid>>;