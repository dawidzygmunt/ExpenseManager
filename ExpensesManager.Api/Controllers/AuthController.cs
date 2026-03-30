using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesManager.Api.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController(IMediator mediator) : ControllerBase
  {
    [HttpPost("login")]
    public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
    {
      var command = new LoginCommand(request.Email, request.Password);
      var response = await mediator.Send(command);
      return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterEndpoint([FromBody] RegisterRequest request)
    {
      var command = new RegisterCommand(request.Email, request.Password);
      var response = await mediator.Send(command);
      return Ok(response);
    }
  }
  
}