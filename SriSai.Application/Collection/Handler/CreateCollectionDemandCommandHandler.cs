using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Handler
{
    public class
        CreateCollectionDemandCommandHandler : IRequestHandler<CreateCollectionDemandCommand, ErrorOr<IList<Guid>>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public CreateCollectionDemandCommandHandler(IUnitOfWork unitOfWork,
            IRepository<ApartmentEntity> apartmentRepository)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<IList<Guid>>> Handle(CreateCollectionDemandCommand request,
            CancellationToken cancellationToken)
        {
            IEnumerable<ApartmentEntity> apartmentIds =
                await _unitOfWork.Repository<ApartmentEntity>().FindAllForConditionAsync(x =>
                    request.ApartmentName.Contains(x.ApartmentNumber));

            IEnumerable<ApartmentEntity> apartmentEntities =
                apartmentIds as ApartmentEntity[] ?? apartmentIds.ToArray();
            if (apartmentEntities.Any())
            {
                await _unitOfWork.BeginTransactionAsync();
                IList<Guid> feeCollectionIds = new List<Guid>();
                foreach (ApartmentEntity apartment in apartmentEntities)
                {
                    FeeCollectionEntity feeCollection = new()
                    {
                        ApartmentId = apartment.Id,
                        Amount = request.Amount,
                        RequestForDate = request.RequestForDate,
                        DueDate = request.DueDate,
                        ForWhat = request.ForWhat,
                        Comment = request.Comment
                    };
                    await _unitOfWork.Repository<FeeCollectionEntity>()
                        .AddAsync(feeCollection);
                    feeCollectionIds.Add(feeCollection.Id);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return feeCollectionIds.ToList();
            }

            return Error.NotFound("Apartment not found");
        }
    }
}