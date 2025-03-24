using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Dtos
{
    public record UnpaidFeeResultDto(
        string Id,
        string ApartmentNumber,
        decimal Amount,
        decimal RemainingAmount,
        DateOnly RequestForDate,
        DateOnly DueDate,
        CollectionType ForWhat,
        string? Comment);
}