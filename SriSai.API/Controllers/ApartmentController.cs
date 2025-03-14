using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Building;
using SriSai.Application.Building.Command;
using SriSai.Application.Users.Query;

namespace SriSai.API.Controllers
{
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

        [HttpPost("Apartment")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> CreateApartment(CreateApartmentDto dto)
        {
            ValidationResult? validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            CreateApartmentCommand command = new(
                dto.ApartmentNumber,
                dto.OwnerId,
                dto.RenterId
            );

            ErrorOr<Guid> result = await _mediator.Send(command);

            return result.Match(
                apartmentId => Ok(apartmentId),
                errors => Problem(string.Join(", ", errors.Select(e => e.Description)))
            );
        }

        [HttpPost("WithRoles")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetUsersWithSpecifiedRoles([FromBody] List<string> roles)
        {
            GetUserWithSpecifiedRoleQuery query = new(roles);
            List<UserWithRoleResponse> result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}