using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Handler;

public class GetExpensesByMonthQueryHandler
    : IRequestHandler<GetExpensesByMonthQuery, ErrorOr<List<ExpenseResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetExpensesByMonthQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<List<ExpenseResponseDto>>> Handle(
        GetExpensesByMonthQuery query,
        CancellationToken cancellationToken)
    {
        var expenses = await _unitOfWork.Repository<ExpenseEntity>()
            .FindAllForConditionAsync(e =>
                e.Date.Month == query.Month &&
                e.Date.Year == query.Year);

        return expenses.Select(e => new ExpenseResponseDto(
            e.Id,
            e.Name,
            e.Type,
            e.Amount,
            e.Date
        )).ToList();
    }
}