using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Handler
{
    public class GetUserPaymentsQueryHandler
        : IRequestHandler<GetUserPaymentsQuery, ErrorOr<List<UserPaymentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserPaymentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<List<UserPaymentDto>>> Handle(
            GetUserPaymentsQuery request,
            CancellationToken cancellationToken)
        {
            var payments = await _unitOfWork.Repository<Payment>().FindAllWithIncludeAsync(
                p => p.FeeCollection.Apartment.Owner.Id == request.UserId,
                p => p.FeeCollection,
                p => p.FeeCollection.Apartment);

            return payments.Select(p => new UserPaymentDto(
                p.Id,
                p.Amount,
                p.PaidDate,
                p.FeeCollectionId,
                p.PaymentMethod,
                p.FeeCollection.Apartment.ApartmentNumber,
                p.FeeCollection.ForWhat,
                p.FeeCollection.Comment
            )).ToList();
        }
    }
}