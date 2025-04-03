using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Dtos;

public record ExpenseResponseDto(
    Guid Id,
    string Name,
    ExpenseType Type,
    decimal Amount,
    DateOnly Date);