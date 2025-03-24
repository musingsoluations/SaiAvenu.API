using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SriSai.API.DTOs.Collection;
using SriSai.Application.Collection.Command;

namespace SriSai.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
    {
        var command = new CreateExpenseCommand(
            dto.Name,
            dto.Type,
            dto.Amount,
            dto.Date);

        var result = await _mediator.Send(command);
        return result.Match(
            id => CreatedAtAction(nameof(CreateExpense), new { id }, id),
            errors => Problem(string.Join(", ", errors.Select(e => e.Code))));
    }
}
