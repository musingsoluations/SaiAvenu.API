using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Messaging.Command;
using SriSai.Domain.Entity.Building;

namespace SriSai.Application.Messaging.Handler
{
    public class PaymentReminderCommandHandler : IRequestHandler<PaymentReminderCommand, ErrorOr<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentReminderCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        public async Task<ErrorOr<Unit>> Handle(PaymentReminderCommand request, CancellationToken cancellationToken)
        {
            string mobileNumber = string.Empty;
            string name = string.Empty;

            ApartmentEntity? result = await _unitOfWork.Repository<ApartmentEntity>().FindOneWithIncludeAsync
                (x => x.ApartmentNumber == request.ApartmentName, x => x.Owner);

            if (result == null)
            {
                return Error.NotFound("Apartment not found");
            }

            mobileNumber = result.Owner.Mobile;
            name = $"{result.Owner.FirstName} {result.Owner.LastName}";

            SendMessageCommand command = new()
            {
                MobileNumber = mobileNumber,
                //Dear {{1}},
                //A demand of {{2}} for {{3}} is created for the month of {{4}}. 
                //The due date for payment is {{5}}. 
                //Please try to make the payment ASAP. 
                //You can transfer to the account details shared in the group or the 
                //UPI @ {{6}}. You can see all the payments made and expenses incurred online @ {{7}}.

                MessageBody = new[]
                {
                    name, request.RequiredAmount, request.RequiredFor, request.ForMonth, request.PaymentDueDate,
                    "earavind-1@okaxis", "https://srisai.dotplus.app/"
                },
                TemplateName = "srisai_2"
            };

            return await _mediator.Send(command, cancellationToken);
        }
    }
}