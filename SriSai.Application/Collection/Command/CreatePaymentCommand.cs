using ErrorOr;
using MediatR;

namespace SriSai.Application.Collection.Command
{
    public record CreatePaymentCommand(
        decimal Amount,
        DateTime PaymentDate,
        Guid FeeCollectionId,
        string PaymentMethod) : IRequest<ErrorOr<Guid>>;
}
