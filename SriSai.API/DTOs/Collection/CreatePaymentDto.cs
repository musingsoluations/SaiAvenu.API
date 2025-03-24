namespace SriSai.API.DTOs.Collection
{
    public sealed record CreatePaymentDto(
        decimal Amount,
        DateOnly PaymentDate,
        Guid FeeCollectionId,
        string PaymentMethod);
}