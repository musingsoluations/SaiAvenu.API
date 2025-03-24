using SriSai.Domain.Entity.Collection;

namespace SriSai.API.DTOs.Collection;

public record CreateExpenseDto(
    string Name,
    ExpenseType Type,
    decimal Amount,
    DateOnly Date); 
