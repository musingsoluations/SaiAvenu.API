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
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator, ILogger<ExpenseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("monthly")]
        [Authorize]
        public async Task<IActionResult> GetExpensesByMonth(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            GetExpensesByMonthQuery query = new(month, year);
            ErrorOr<List<ExpenseResponseDto>> result = await _mediator.Send(query);
            return result.Match(
                expenses => Ok(expenses),
                errors => Problem(string.Join(", ", errors.Select(e => e.Code))));
        }

        [HttpPost("create")]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
        {
            _logger.LogInformation("Creating expense");
            string? userGuid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            CreateExpenseCommand command = new(
                dto.Name,
                dto.Type,
                dto.Amount,
                dto.Date,
                Guid.Parse(userGuid ?? string.Empty)
            );

            ErrorOr<Guid> result = await _mediator.Send(command);
            return result.Match(
                id => CreatedAtAction(nameof(CreateExpense), new { id }, id),
                errors => Problem(string.Join(", ", errors.Select(e => e.Code))));
        }
    }
}