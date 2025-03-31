using ErrorOr;
using MediatR;
using SriSai.Application.Collection.Command;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Domain.Entity.Collection;
using SriSai.Domain.Interface;

namespace SriSai.Application.Collection.Handler
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ErrorOr<Guid>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExpenseCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<Guid>> Handle(
            CreateExpenseCommand command,
            CancellationToken cancellationToken)
        {
            ExpenseEntity entity = new()
            {
                Name = command.Name,
                Type = command.Type,
                Amount = command.Amount,
                Date = command.Date,
                CreatedBy = command.CreatedBy,
                CreatedDateTime = _dateTimeProvider.GetUtcNow(),
                IsDeleted = false
            };

            await _unitOfWork.Repository<ExpenseEntity>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}