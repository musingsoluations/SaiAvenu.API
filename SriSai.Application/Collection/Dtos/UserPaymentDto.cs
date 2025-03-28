using SriSai.Domain.Entity.Collection;
using System;

namespace SriSai.Application.Collection.Dtos
{
    public record UserPaymentDto(
        Guid Id,
        decimal Amount,
        DateOnly PaidDate,
        Guid FeeCollectionId,
        string PaymentMethod,
        string ApartmentNumber,
        CollectionType ForWhat,
        string Comment
    );
}