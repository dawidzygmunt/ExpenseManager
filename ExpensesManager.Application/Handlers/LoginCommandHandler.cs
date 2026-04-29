using ExpensesManager.Application.Commands;
using ExpensesManager.Application.DTOs;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class LoginCommandHandler(
    IJwtService jwtService,
    IUserRepository userRepository
) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Email is required", nameof(request.Email));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Password is required", nameof(request.Password));
        }

        if (request.Password.Length < 6)
        {
            throw new ArgumentException("Password is too short", nameof(request.Password));
        }

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

        var roles = await userRepository.GetRolesAsync(user);
        var accessToken = jwtService.GenerateAccessToken(user, roles);
        var refreshToken = jwtService.GenerateRefreshToken();


        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName);

        return new LoginResponse(accessToken, refreshToken.Token, refreshToken.ExpiryTime, userDto);
    }
}