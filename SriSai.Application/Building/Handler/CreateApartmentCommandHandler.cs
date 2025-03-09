using ErrorOr;
using MediatR;
using SriSai.Application.Building.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Errors;

namespace SriSai.Application.Building.Handler;

public class CreateApartmentCommandHandler
    : IRequestHandler<CreateApartmentCommand, ErrorOr<Guid>>
{
    private readonly IRepository<Apartment> _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateApartmentCommandHandler(
        IRepository<Apartment> apartmentRepository,
        IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateApartmentCommand command,
        CancellationToken cancellationToken)
    {
        var existingApartment = await _apartmentRepository
            .ListAsync(x => x.ApartmentNumber == command.ApartmentNumber);
        if (existingApartment.Any()) return Error.Conflict(PreDefinedErrorsForBuilding.ApartmentAlreadyExist);
        var apartment = new Apartment
        {
            ApartmentNumber = command.ApartmentNumber,
            OwnerId = command.OwnerId,
            RenterId = command.RenterId
        };
        await _apartmentRepository.AddAsync(apartment);
        await _unitOfWork.SaveChangesAsync();
        return apartment.Id;
    }
}