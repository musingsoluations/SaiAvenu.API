using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Command;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;

namespace SriSai.Application.Collection.Handler;

public class GetTotalAmountsCommandHandler : IRequestHandler<GetTotalAmountsCommand, ErrorOr<TotalAmountsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTotalAmountsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<TotalAmountsDto>> Handle(
        GetTotalAmountsCommand command,
        CancellationToken cancellationToken)
    {
        var payments = await _unitOfWork.Repository<Payment>()
        .FindAllForConditionAsync(p => !p.IsDeleted);
        var totalPayments = payments.Sum(p => p.Amount);

        var expenses = await _unitOfWork.Repository<ExpenseEntity>()
            .FindAllForConditionAsync(x => !x.IsDeleted);
        var totalExpenses = expenses.Sum(e => e.Amount);

        var carryForwardPayments = await _unitOfWork.Repository<CarryForwardPayment>()
            .FindAllForConditionAsync(x => !x.IsDeleted);
        var totalCarryForwardPayments = carryForwardPayments.Sum(c => c.Amount);

        return new TotalAmountsDto(
            totalPayments,
            totalExpenses,
            totalCarryForwardPayments
        );
    }
}