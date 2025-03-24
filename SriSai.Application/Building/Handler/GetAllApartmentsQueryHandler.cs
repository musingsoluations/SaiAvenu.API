using ErrorOr;
using MediatR;
using SriSai.Application.Building.Query;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Building;

namespace SriSai.Application.Building.Handler
{
    public class GetAllApartmentsQueryHandler
        : IRequestHandler<GetAllApartmentsQuery, ErrorOr<List<ListApartmentsQueryData>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllApartmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<List<ListApartmentsQueryData>>> Handle(
            GetAllApartmentsQuery query,
            CancellationToken cancellationToken)
        {
            IEnumerable<ApartmentEntity> apartments = await _unitOfWork.Repository<ApartmentEntity>().ListAllForConditionWithIncludeAsync(
                x => x.Owner,
                x => x.Renter
            );

            if (!apartments.Any())
            {
                return Error.NotFound("No apartments found");
            }

            List<ListApartmentsQueryData> apartmentsData = apartments.Select(x => new ListApartmentsQueryData
            {
                ApartmentNumber = x.ApartmentNumber,
                OwnerId = x.OwnerId,
                RenterId = x.RenterId,
                OwnerName = x.Owner != null ? $"{x.Owner.FirstName} {x.Owner.LastName}".Trim() : "Unknown Owner",
                RenterName = x.Renter != null ? $"{x.Renter.FirstName} {x.Renter.LastName}".Trim() : null
            }).ToList();

            return apartmentsData;
        }
    }
}