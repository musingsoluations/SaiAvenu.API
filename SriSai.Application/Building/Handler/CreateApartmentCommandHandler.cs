using ErrorOr;
using MediatR;
using SriSai.Application.Building.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Errors;

namespace SriSai.Application.Building.Handler
{
    public class CreateApartmentCommandHandler
        : IRequestHandler<CreateApartmentCommand, ErrorOr<Guid>>
    {
        private readonly IRepository<ApartmentEntity> _apartmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateApartmentCommandHandler(
            IRepository<ApartmentEntity> apartmentRepository,
            IUnitOfWork unitOfWork)
        {
            _apartmentRepository = apartmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Guid>> Handle(
            CreateApartmentCommand command,
            CancellationToken cancellationToken)
        {
            ApartmentEntity? existingApartment = await _apartmentRepository
                .FindOneAsync(x => x.ApartmentNumber == command.ApartmentNumber);
            if (existingApartment is not null)
            {
                return Error.Conflict(PreDefinedErrorsForBuilding.ApartmentAlreadyExist);
            }

            ApartmentEntity apartmentEntity = new()
            {
                ApartmentNumber = command.ApartmentNumber, OwnerId = command.OwnerId, RenterId = command.RenterId
            };
            await _apartmentRepository.AddAsync(apartmentEntity);
            await _unitOfWork.SaveChangesAsync();
            return apartmentEntity.Id;
        }
    }
}