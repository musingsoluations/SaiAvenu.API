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
        private readonly IRepository<Apartment> _apartmentRepository;

        public GetAllApartmentsQueryHandler(IRepository<Apartment> apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public async Task<ErrorOr<List<ListApartmentsQueryData>>> Handle(
            GetAllApartmentsQuery query,
            CancellationToken cancellationToken)
        {
            var apartments = await _apartmentRepository.ListAllAsync(
                x => x.Owner,
                x => x.Renter
            );

            if (!apartments.Any())
            {
                return Error.NotFound("No apartments found");
            }

            var apartmentsData = apartments.Select(x => new ListApartmentsQueryData
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