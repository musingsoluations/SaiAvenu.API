using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;
using SriSai.Domain.Interface;

namespace SriSai.Application.Collection.Handler
{
    public class CreatePaymentCommandHandler :
        IRequestHandler<CreatePaymentCommand, ErrorOr<Guid>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<Guid>> Handle(
            CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            Payment payment = new()
            {
                Amount = command.Amount,
                PaidDate = command.PaymentDate,
                FeeCollectionId = command.FeeCollectionId,
                PaymentMethod = command.PaymentMethod,
                CreatedBy = command.CreatedBy,
                CreatedDateTime = _dateTimeProvider.GetUtcNow(),
                IsDeleted = false
            };
            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return payment.Id;
        }
    }
}