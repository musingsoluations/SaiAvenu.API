using FluentValidation;
using SriSai.Domain.Entity.Collection;

namespace SriSai.API.DTOs.Collection.Validation
{
    public class CreateCollectionDemandDtoValidator : AbstractValidator<CreateCollectionDemandDto>
    {
        public CreateCollectionDemandDtoValidator()
        {
            RuleFor(dto => dto.ApartmentName)
                .NotEmpty().WithMessage("At least one apartment must be specified")
                .Must(names => names.All(n => !string.IsNullOrWhiteSpace(n)))
                .WithMessage("Apartment names cannot be empty");

            RuleFor(dto => dto.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            // RuleFor(dto => dto.RequestForDate)
            //     .GreaterThanOrEqualTo(DateTime.Today)
            //     .WithMessage("Request date cannot be in the past");

            // RuleFor(dto => dto.DueDate)
            //     .GreaterThan(dto => dto.RequestForDate)
            //     .WithMessage("Due date must be after request date");

            // RuleFor(dto => dto.PaidDate)
            //     .GreaterThanOrEqualTo(dto => dto.DueDate)
            //     .When(dto => dto.IsPaid)
            //     .WithMessage("Paid date must be on or after due date");

            RuleFor(dto => dto.ForWhat)
                .IsInEnum().WithMessage("Invalid collection type specified");

            RuleFor(dto => dto.Comment)
                .NotEmpty()
                .When(dto => dto.ForWhat == CollectionType.AdhocExpense)
                .WithMessage("Comment is required for Adhoc Expense collections");
        }
    }
}