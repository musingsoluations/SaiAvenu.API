using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Dtos
{
    public record UnpaidFeeResultDto(
        string Id,
        string ApartmentNumber,
        decimal Amount,
        decimal RemainingAmount,
        DateTime RequestForDate,
        DateTime DueDate,
        CollectionType ForWhat,
        string? Comment);
}