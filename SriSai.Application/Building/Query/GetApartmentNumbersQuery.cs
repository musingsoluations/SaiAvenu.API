using ErrorOr;
using MediatR;

namespace SriSai.Application.Building.Query;

public record GetApartmentNumbersQuery() : IRequest<ErrorOr<List<string>>>;
