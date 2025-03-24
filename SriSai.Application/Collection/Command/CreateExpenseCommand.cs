using ErrorOr;
using MediatR;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Command;

public record CreateExpenseCommand(
    string Name,
    ExpenseType Type,
    decimal Amount,
    DateOnly Date) : IRequest<ErrorOr<Guid>>;
