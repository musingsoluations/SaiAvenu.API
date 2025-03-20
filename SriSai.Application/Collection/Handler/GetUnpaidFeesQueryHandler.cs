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
        private readonly IRepository<FeeCollectionEntity> _repository;

        public GetUnpaidFeesQueryHandler(IRepository<FeeCollectionEntity> repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<List<UnpaidFeeResultDto>>> Handle(
            GetUnpaidFeesQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<FeeCollectionEntity> fees = await _repository.FindAllWithIncludeAsync(
                f => !f.IsPaid,
                q => q.Apartment);

            return fees.Select(f => new UnpaidFeeResultDto(
                f.Id.ToString(),
                f.Apartment.ApartmentNumber,
                f.Amount,
                f.RequestForDate,
                f.DueDate,
                f.ForWhat,
                f.Comment
            )).ToList();
        }
    }
}