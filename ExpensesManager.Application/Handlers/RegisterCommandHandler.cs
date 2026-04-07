using ExpensesManager.Application.Commands;
using ExpensesManager.Application.DTOs;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpensesManager.Application.Handlers;

public class RegisterCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository
) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            throw new Exception("User with this email already exists");
        }

        var hashedPassword = passwordHasher.Hash(request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = "xd",
            LastName = "xd",
            PasswordHash = hashedPassword,
        };
        var newUser = await userRepository.AddAsync(user);

        var userDto = new UserDto(
            Guid.NewGuid(),
            newUser.Email,
            newUser.FirstName,
            newUser.LastName);

        return new RegisterResponse(true, userDto);
    }
}