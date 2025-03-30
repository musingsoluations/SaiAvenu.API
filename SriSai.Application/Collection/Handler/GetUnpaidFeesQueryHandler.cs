using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Handler
{
    public class GetUnpaidFeesQueryHandler
        : IRequestHandler<GetUnpaidFeesQuery, ErrorOr<List<UnpaidFeeResultDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUnpaidFeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<List<UnpaidFeeResultDto>>> Handle(
            GetUnpaidFeesQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<FeeCollectionEntity> fees = await _unitOfWork.Repository<FeeCollectionEntity>()
                .FindAllWithIncludeAsync(
                    f => f.Amount > f.Payments.Sum(p => p.Amount), q => q.Apartment, q => q.Payments);

            return fees.Select(f => new UnpaidFeeResultDto(
                    f.Id.ToString(),
                    f.Apartment.ApartmentNumber,
                    f.Amount,
                    f.RemainingAmount,
                    f.RequestForDate,
                    f.DueDate,
                    f.ForWhat,
                    f.Comment
                )).OrderBy(f => f.ApartmentNumber)
                .ToList();
        }
    }
}