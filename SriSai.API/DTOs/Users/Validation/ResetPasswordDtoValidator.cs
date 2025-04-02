using FluentValidation;
using SriSai.API.DTOs.Users;

namespace SriSai.API.DTOs.Users.Validation
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile number is required")
                .Matches(@"^\d{10}$").WithMessage("Mobile number must be 10 digits");
        }
    }
}