using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Command;
using SriSai.Domain.Entity.Collection;
using SriSai.Application.interfaces.Reposerty;

namespace SriSai.Application.Collection.Handler
{
    public class CreatePaymentCommandHandler :
        IRequestHandler<CreatePaymentCommand, ErrorOr<Guid>>
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IRepository<Payment> paymentRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Guid>> Handle(
            CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                Amount = command.Amount,
                PaidDate = command.PaymentDate,
                FeeCollectionId = command.FeeCollectionId,
                PaymentMethod = command.PaymentMethod,
            };
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return payment.Id;
        }
    }
}
