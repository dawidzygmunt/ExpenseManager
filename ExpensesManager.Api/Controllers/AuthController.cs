using ExpensesManager.Api.DTO;
using ExpensesManager.Application.Commands;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using LoginRequest = ExpensesManager.Api.DTO.LoginRequest;

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
      var command = new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName);
      var response = await mediator.Send(command);
      return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutEndpoint()
    {
      var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      var command = new LogoutCommand(accessToken);
      var response = await mediator.Send(command);
      
      Response.Cookies.Delete("refreshToken");

      return Ok(response);
    }
  }
  
}