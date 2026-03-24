using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Dto;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class LoginCommandHandler(IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
{
  private readonly IUserRepository _userRepository;
  private readonly ITokenService _tokenService;

  public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
  {
    // Get user from db, and verify hashed passw
  }
}