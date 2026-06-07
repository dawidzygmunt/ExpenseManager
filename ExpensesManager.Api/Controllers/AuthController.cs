using ExpensesManager.Api.DTO;
using ExpensesManager.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = ExpensesManager.Api.DTO.LoginRequest;

namespace ExpensesManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await mediator.Send(command);

        var isDev = !Request.IsHttps;
        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !isDev,
            SameSite = isDev ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = response.RefreshTokenExpiryTime
        });

        return Ok(new { response.Token, response.UserDto });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterEndpoint([FromBody] RegisterRequest request)
    {
        var command =
            new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName);
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

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken)
            || string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized();

        var response = await mediator.Send(new RefreshTokenCommand(refreshToken));
        var isDev = !Request.IsHttps;

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !isDev,
            SameSite = isDev ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = response.RefreshTokenExpiryTime,
            Path = "/api/auth"
        });

        return Ok(new { response.Token, response.UserDto });
    }
}