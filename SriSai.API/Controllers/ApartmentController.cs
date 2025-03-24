using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Building;
using SriSai.Application.Building.Command;
using SriSai.Application.Building.Query;
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

        [HttpPost("AddApartment")]
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
                apartmentId => Ok(result.Value),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }

        [HttpGet("GetApartments")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetAllApartments()
        {
            GetAllApartmentsQuery query = new();
            ErrorOr<List<ListApartmentsQueryData>> result = await _mediator.Send(query);
            return result.Match(
                apartments => Ok(apartments),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }

        [HttpGet("GetApartmentNumbers")]
        [Authorize("AdminOnly")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetApartmentNumbers()
        {
            ErrorOr<List<string>> result = await _mediator.Send(new GetApartmentNumbersQuery());
            return result.Match(
                numbers => Ok(numbers),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }


        [HttpPost("userwithroles")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetUsersWithSpecifiedRoles([FromBody] List<string> roles)
        {
            GetUserWithSpecifiedRoleQuery query = new(roles);
            List<UserWithRoleResponse> result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}