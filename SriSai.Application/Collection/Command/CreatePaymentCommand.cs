using ErrorOr;
using MediatR;

namespace SriSai.Application.Collection.Command
{
    public record CreatePaymentCommand(
        decimal Amount,
        DateOnly PaymentDate,
        Guid FeeCollectionId,
        string PaymentMethod,
        Guid CreatedBy) : IRequest<ErrorOr<Guid>>;
}