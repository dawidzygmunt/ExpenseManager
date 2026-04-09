using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Interfaces;
using Moq;

namespace ExpensesManager.Api.UnitTest.Handlers;

public class LoginCommandHandlerTests
{
    private readonly LoginCommandHandler _sut;
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly Mock<IUserRepository> _userRepository = new();


    public LoginCommandHandlerTests()
    {
        _sut = new LoginCommandHandler(_jwtService.Object, _userRepository.Object);
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenInvalidPassword_ThrowUnauthorizedAccessException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        _userRepository.Setup(x => x.CheckPasswordAsync(
            It.Is<User>(u => u.Email == request.Email), request.Password));
        
        // Act & Assert
        await  Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.Handle(request, CancellationToken.None));
    }
}