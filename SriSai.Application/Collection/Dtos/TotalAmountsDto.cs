namespace SriSai.Application.Collection.Dtos;

public record TotalAmountsDto(
    decimal TotalPayments,
    decimal TotalExpenses,
    decimal TotalCarryForwardPayments
); 