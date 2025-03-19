using MediatR;
using Microsoft.AspNetCore.Mvc;
using SriSai.Application.Collection.Command;
using SriSai.API.DTOs.Collection;

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

        [HttpPost("demand")]
        public async Task<IActionResult> CreateCollectionDemand(CreateCollectionDemandDto dto)
        {
            var command = new CreateCollectionDemandCommand
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

            var result = await _mediator.Send(command);

            return result.Match(
                collectionIds => Ok(collectionIds),
                errors => Problem(string.Join(", ", errors.Select(e => e.Code))));
        }
    }
}
