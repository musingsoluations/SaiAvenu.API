using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Building;
using SriSai.Application.Building.Command;

namespace SriSai.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApartmentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateApartmentDto> _validator;

    public ApartmentController(
        IMediator mediator,
        IValidator<CreateApartmentDto> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateApartment(CreateApartmentDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        }

        var command = new CreateApartmentCommand(
            ApartmentNumber: dto.ApartmentNumber,
            OwnerId: dto.OwnerId,
            RenterId: dto.RenterId
        );

        var result = await _mediator.Send(command);

        return result.Match(
            apartmentId => Ok(apartmentId),
            errors => Problem(string.Join(", ", errors.Select(e => e.Description)))
        );
    }
}