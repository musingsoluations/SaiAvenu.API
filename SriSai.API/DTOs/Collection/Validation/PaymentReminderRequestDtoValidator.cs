using FluentValidation;
using SriSai.API.DTOs.Collection;

namespace SriSai.API.DTOs.Collection.Validation
{
    public class PaymentReminderRequestDtoValidator : AbstractValidator<PaymentReminderRequestDto>
    {
        public PaymentReminderRequestDtoValidator()
        {
            RuleFor(x => x.ApartmentName)
                .NotEmpty()
                .WithMessage("Apartment name is required")
                .MaximumLength(100)
                .WithMessage("Apartment name must not exceed 100 characters");

            RuleFor(x => x.RequiredAmount)
                .GreaterThan(0)
                .WithMessage("Required amount must be greater than 0");

            RuleFor(x => x.RequiredFor)
                .NotEmpty()
                .WithMessage("Required for field is required")
                .MaximumLength(200)
                .WithMessage("Required for field must not exceed 100 characters");

            RuleFor(x => x.ForMonth)
                .NotEmpty()
                .WithMessage("For month is required");

            RuleFor(x => x.PaymentDueDate)
                .NotEmpty()
                .WithMessage("Payment due date is required")
                .GreaterThanOrEqualTo(x => x.ForMonth)
                .WithMessage("Payment due date must be on or after the month");
        }
    }
}