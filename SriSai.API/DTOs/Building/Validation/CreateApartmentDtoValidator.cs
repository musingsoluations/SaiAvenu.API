using FluentValidation;
using SriSai.API.DTOs.Building;

namespace SriSai.API.DTOs.Building.Validation;

public class CreateApartmentDtoValidator : AbstractValidator<CreateApartmentDto>
{
    public CreateApartmentDtoValidator()
    {
        RuleFor(x => x.ApartmentNumber).NotEmpty().MaximumLength(10);
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}