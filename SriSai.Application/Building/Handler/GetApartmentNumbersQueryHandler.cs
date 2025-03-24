using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Building.Query;
using SriSai.Domain.Entity.Building;

namespace SriSai.Application.Building.Handler;

public class GetApartmentNumbersQueryHandler
    : IRequestHandler<GetApartmentNumbersQuery, ErrorOr<List<string>>>
{
    //private readonly IRepository<ApartmentEntity> _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetApartmentNumbersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<List<string>>> Handle(
        GetApartmentNumbersQuery request,
        CancellationToken cancellationToken)
    {
        var apartments = await _unitOfWork.Repository<ApartmentEntity>().ListAllAsync();
        return apartments.Select(a => a.ApartmentNumber).ToList();
    }
}
