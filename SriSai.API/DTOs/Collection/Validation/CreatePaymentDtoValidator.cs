using FluentValidation;
using SriSai.API.DTOs.Collection;

namespace SriSai.API.DTOs.Collection.Validation
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.PaymentDate).NotEmpty();
            RuleFor(x => x.FeeCollectionId).NotEmpty();
            RuleFor(x => x.PaymentMethod).NotEmpty().MaximumLength(50);
        }
    }
}
