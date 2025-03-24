using ErrorOr;
using MediatR;
using SriSai.Domain.Entity.Collection;
using SriSai.Application.Collection.Command;
using SriSai.Application.interfaces.Reposerty;

namespace SriSai.Application.Collection.Handler;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ErrorOr<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateExpenseCommand command,
        CancellationToken cancellationToken)
    {
        var entity = new ExpenseEntity
        {
            Name = command.Name,
            Type = command.Type,
            Amount = command.Amount,
            Date = command.Date
        };

        await _unitOfWork.Repository<ExpenseEntity>().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.Id;
    }
}
