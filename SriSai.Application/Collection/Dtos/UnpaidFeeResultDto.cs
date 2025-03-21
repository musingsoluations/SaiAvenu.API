namespace SriSai.Application.Collection.Dtos;

public record UnpaidFeeResultDto(
    string ApartmentNumber,
    decimal Amount,
    DateTime RequestForDate,
    DateTime DueDate,
    SriSai.Domain.Entity.Collection.CollectionType ForWhat,
    string? Comment);
