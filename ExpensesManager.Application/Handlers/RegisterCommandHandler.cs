using ExpensesManager.Application.Commands;
using ExpensesManager.Application.DTOs;
using ExpensesManager.Application.Exceptions;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class RegisterCommandHandler(
    IUserRepository userRepository
) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null) throw new ConflictException("User with this email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        var newUser = await userRepository.AddAsync(user, request.Password);


        var userDto = new UserDto(
            newUser.Id,
            newUser.Email!,
            newUser.FirstName,
            newUser.LastName);

        return new RegisterResponse(true, userDto);
    }
}