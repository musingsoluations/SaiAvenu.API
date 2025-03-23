namespace SriSai.API.DTOs.Collection
{
    public record UnpaidFeeDto(
        string Id,
        string ApartmentNumber,
        decimal Amount,
        DateTime RequestForDate,
        DateTime DueDate,
        string ForWhat,
        string? Comment);
}