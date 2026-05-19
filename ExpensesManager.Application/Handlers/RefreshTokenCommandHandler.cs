using ExpensesManager.Application.Commands;
using ExpensesManager.Application.DTOs;
using ExpensesManager.Application.Exceptions;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class RefreshTokenCommandHandler(
    IJwtService jwtService,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository
) : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new UnauthorizedException("Missing refresh token");

        var stored = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken)
                     ?? throw new UnauthorizedException("Invalid refresh token");

        if (!stored.IsActive)
            throw new UnauthorizedException("Refresh token expired or revoked");

        // rotacja: stary unieważniamy, generujemy nowy
        stored.RevokedAt = DateTime.UtcNow;
        await refreshTokenRepository.UpdateAsync(stored);

        var user = await userRepository.GetByIdAsync(stored.UserId)
                   ?? throw new UnauthorizedException("User not found");

        var roles = await userRepository.GetRolesAsync(user);
        var newAccess = jwtService.GenerateAccessToken(user, roles);
        var newRefresh = jwtService.GenerateRefreshToken();

        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = newRefresh.Token,
            ExpiryTime = newRefresh.ExpiryTime,
            UserId = user.Id
        });

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName);
        return new LoginResponse(newAccess, newRefresh.Token, newRefresh.ExpiryTime, userDto);
    }
}