using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;

namespace SriSai.Application.Collection.Query;

public record GetExpensesByMonthQuery(int Month, int Year)
    : IRequest<ErrorOr<List<ExpenseResponseDto>>>;
