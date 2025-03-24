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
        private readonly IUnitOfWork _unitOfWork;

        public CreateApartmentCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Guid>> Handle(
            CreateApartmentCommand command,
            CancellationToken cancellationToken)
        {
            ApartmentEntity? existingApartment = await _unitOfWork.Repository<ApartmentEntity>()
                .FindOneAsync(x => x.ApartmentNumber == command.ApartmentNumber);
            if (existingApartment is not null)
            {
                return Error.Conflict(PreDefinedErrorsForBuilding.ApartmentAlreadyExist);
            }

            ApartmentEntity apartmentEntity = new()
            {
                ApartmentNumber = command.ApartmentNumber,
                OwnerId = command.OwnerId,
                RenterId = command.RenterId
            };
            await _unitOfWork.Repository<ApartmentEntity>().AddAsync(apartmentEntity);
            await _unitOfWork.SaveChangesAsync();
            return apartmentEntity.Id;
        }
    }
}