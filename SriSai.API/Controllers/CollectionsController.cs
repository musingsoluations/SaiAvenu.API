using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Collection;
using SriSai.Application.Collection.Command;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;

namespace SriSai.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("unpaid")]
        //[Authorize("AdminOnly")]
        [ProducesResponseType(typeof(List<UnpaidFeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnpaidFees()
        {
            ErrorOr<List<UnpaidFeeResultDto>> result = await _mediator.Send(new GetUnpaidFeesQuery());
            return result.Match(
                fees => Ok(fees),
                errors => Problem(string.Join(", ", errors.Select(e => e.Code)))
            );
        }

        [HttpPost("demand")]
        public async Task<IActionResult> CreateCollectionDemand(CreateCollectionDemandDto dto)
        {
            CreateCollectionDemandCommand command = new()
            {
                ApartmentName = dto.ApartmentName,
                Amount = dto.Amount,
                RequestForDate = dto.RequestForDate,
                DueDate = dto.DueDate,
                PaidDate = dto.PaidDate,
                IsPaid = dto.IsPaid,
                ForWhat = dto.ForWhat,
                Comment = dto.Comment
            };

            ErrorOr<IList<Guid>> result = await _mediator.Send(command);

            return result.Match(
                collectionIds => Ok(collectionIds),
                errors => Problem(string.Join(", ", errors.Select(e => e.Code))));
        }
    }
}