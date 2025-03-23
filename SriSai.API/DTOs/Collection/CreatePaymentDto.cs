using System;

namespace SriSai.API.DTOs.Collection
{
    public sealed record CreatePaymentDto(
        decimal Amount,
        DateTime PaymentDate,
        Guid FeeCollectionId,
        string PaymentMethod);
}
