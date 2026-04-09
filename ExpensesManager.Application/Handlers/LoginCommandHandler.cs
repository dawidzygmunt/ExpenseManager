using ExpensesManager.Application.Commands;
using ExpensesManager.Application.DTOs;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class LoginCommandHandler(
    IJwtService jwtService,
    IUserRepository userRepository
) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var isPasswordValid = await userRepository.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // TODO: Replace with ASP entity
        var roles = new List<string>();
        var accessToken = jwtService.GenerateAccessToken(user, roles);
        var refreshToken = jwtService.GenerateRefreshToken();

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName);
        
        return new LoginResponse(accessToken, refreshToken, userDto);
    }
}