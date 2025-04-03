using ErrorOr;
using MediatR;

namespace SriSai.Application.Messaging.Command
{
    public class PaymentReminderCommand : IRequest<ErrorOr<Unit>>
    {
        //Dear {{1}},
        //A demand of {{2}} for {{3}} is created for the month of {{4}}. 
        //The due date for payment is {{5}}. 
        //Please try to make the payment ASAP. 
        //You can transfer to the account details shared in the group or the 
        //UPI @ {{6}}. You can see all the payments made and expenses incurred online @ {{7}}.

        public string ApartmentName { get; set; } = string.Empty;
        public string RequiredAmount { get; set; } = string.Empty;
        public string RequiredFor { get; set; } = string.Empty;
        public string ForMonth { get; set; } = string.Empty;
        public string PaymentDueDate { get; set; } = string.Empty;

    }
}