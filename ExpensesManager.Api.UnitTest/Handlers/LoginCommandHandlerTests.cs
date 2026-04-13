using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Application.Responses;
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
    public async Task LoginCommandHandler_Handle_WhenUserDoesNotExist_ThrowUnauthorizedAccessException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        _userRepository
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenInvalidPassword_ThrowUnauthorizedAccessException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        var user = new User
        {
            Email = request.Email,
        };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _userRepository.Setup(x => x.CheckPasswordAsync(
            user, request.Password)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenCredentialsAreCorrect_ReturnLoginResponse()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = "mockedFirstName",
            LastName = "mockedLastName",
            PasswordHash = "mockedHashedPassword"
        };
        const string mockedAccessToken = "mockedAccessToken";

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _userRepository.Setup(x => x.CheckPasswordAsync(
            user, request.Password)).ReturnsAsync(true);

        _jwtService.Setup(x => x.GenerateAccessToken(user)).Returns(mockedAccessToken);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(mockedAccessToken, response.AccessToken);
    }
}