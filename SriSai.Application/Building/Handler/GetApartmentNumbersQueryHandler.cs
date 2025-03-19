using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Building.Query;
using SriSai.Domain.Entity.Building;

namespace SriSai.Application.Building.Handler;

public class GetApartmentNumbersQueryHandler
    : IRequestHandler<GetApartmentNumbersQuery, ErrorOr<List<string>>>
{
    private readonly IRepository<ApartmentEntity> _apartmentRepository;

    public GetApartmentNumbersQueryHandler(IRepository<ApartmentEntity> apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<ErrorOr<List<string>>> Handle(
        GetApartmentNumbersQuery request,
        CancellationToken cancellationToken)
    {
        var apartments = await _apartmentRepository.ListAllAsync();
        return apartments.Select(a => a.ApartmentNumber).ToList();
    }
}
