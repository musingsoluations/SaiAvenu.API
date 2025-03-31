using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Collection;
using SriSai.Application.Collection.Command;
using SriSai.Application.Collection.Dtos;
using SriSai.Application.Collection.Query;
using System.Security.Claims;

namespace SriSai.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionsController : ControllerBase
    {
        private readonly ILogger<CollectionsController> _logger;
        private readonly IMediator _mediator;

        public CollectionsController(IMediator mediator, ILogger<CollectionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("unpaid")]
        [Authorize("AdminOnly")]
        //[Authorize("AdminOnly")]
        [ProducesResponseType(typeof(List<UnpaidFeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnpaidFees()
        {
            _logger.LogInformation("Getting unpaid fees");
            ErrorOr<List<UnpaidFeeResultDto>> result = await _mediator.Send(new GetUnpaidFeesQuery());
            return result.Match(
                fees => Ok(fees),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }

        [HttpPost("demand")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> CreateCollectionDemand(CreateCollectionDemandDto dto)
        {
            _logger.LogInformation("Creating collection demand");
            CreateCollectionDemandCommand command = new()
            {
                ApartmentName = dto.ApartmentName,
                Amount = dto.Amount,
                RequestForDate = dto.RequestForDate,
                DueDate = dto.DueDate,
                PaidDate = dto.PaidDate,
                ForWhat = dto.ForWhat,
                Comment = dto.Comment
            };
            ErrorOr<IList<Guid>> result = await _mediator.Send(command);

            return result.Match(
                collectionIds => Ok(collectionIds),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }

        [HttpPost("payment")]
        [Authorize("AdminOnly")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            _logger.LogInformation("Creating payment");
            CreatePaymentCommand command = new(
                dto.Amount,
                dto.PaymentDate,
                dto.FeeCollectionId,
                dto.PaymentMethod);

            ErrorOr<Guid> result = await _mediator.Send(command);

            return result.Match(
                paymentId => Ok(paymentId),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
        }

        [HttpPost("collection-payment")]
        [Authorize]
        public async Task<IActionResult> GetCollectionExpense([FromBody] int year)
        {
            _logger.LogInformation("Getting collection expense");
            GetCollectionPaymentQuery query = new() { Year = year };

            ErrorOr<List<ChartDataItem>> result = await _mediator.Send(query);
            return result.Match(
                chartDataItems => Ok(chartDataItems),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                })
            );
            ;
        }

        [HttpPost("demand-paid-self")]
        [Authorize]
        public async Task<IActionResult> GetSelfPaidCollection([FromBody] int year)
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"Getting self-paid collection for {userId}");
            GetCollectionExpenseSelfQuery query = new()
            {
                Year = year, CurrentUserId = Guid.Parse(userId ?? string.Empty)
            };

            ErrorOr<List<ChartDataItem>> result = await _mediator.Send(query);
            return result.Match(
                chartDataItems => Ok(chartDataItems),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }

        [HttpGet("user-payments")]
        [Authorize]
        [ProducesResponseType(typeof(List<UserPaymentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPayments()
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"Getting user payments for {userId}");
            ErrorOr<List<UserPaymentDto>> result = await _mediator.Send(
                new GetUserPaymentsQuery(Guid.Parse(userId ?? string.Empty)));

            return result.Match(
                payments => Ok(payments),
                errors => new ObjectResult(new ProblemDetails
                {
                    Detail = result.Errors.FirstOrDefault().Description,
                    Extensions = { ["errors"] = result.Errors.FirstOrDefault().Code }
                }));
        }
    }
}