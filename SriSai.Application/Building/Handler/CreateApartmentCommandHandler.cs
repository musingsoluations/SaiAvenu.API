using ErrorOr;
using MediatR;
using SriSai.Application.Building.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Errors;
using SriSai.Domain.Interface;

namespace SriSai.Application.Building.Handler
{
    public class CreateApartmentCommandHandler
        : IRequestHandler<CreateApartmentCommand, ErrorOr<Guid>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreateApartmentCommandHandler(
            IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
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
                RenterId = command.RenterId,
                CreatedBy = command.CreatedById,
                CreatedDateTime = _dateTimeProvider.GetUtcNow(),
                IsDeleted = false
            };
            await _unitOfWork.Repository<ApartmentEntity>().AddAsync(apartmentEntity);
            await _unitOfWork.SaveChangesAsync();
            return apartmentEntity.Id;
        }
    }
}