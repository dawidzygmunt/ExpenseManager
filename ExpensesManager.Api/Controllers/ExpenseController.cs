using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesManager.Api.Controllers;

[ApiController]
[Route("api/expense")]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var query = new GetAllExpensesQuery();
        var response = await mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetExpensesByWorkspaceId(Guid workspaceId, [FromQuery] Guid userId)
    {
        var query = new GetAllExpensesQuery();
        var response = await mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        var query = new GetExpenseByIdQuery(id);
        var response = await mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] AddExpenseCommand command)
    {
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(GetExpenseById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseCommand command)
    {
        await mediator.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        await mediator.Send(new DeleteExpenseCommand(id));
        return NoContent();
    }
}