using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
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
        var roles = new List<string> { "User" };
        const string mockedAccessToken = "mockedAccessToken";
        var mockedRefreshToken = new RefreshToken
        {
            Token =  "mockedRefreshToken",
            ExpiryTime = DateTime.Now.AddMinutes(5),
        };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _userRepository.Setup(x => x.CheckPasswordAsync(
            user, request.Password)).ReturnsAsync(true);
        _userRepository.Setup(x=> x.GetRolesAsync(user)).ReturnsAsync(roles);

        _jwtService.Setup(x => x.GenerateAccessToken(user, roles)).Returns(mockedAccessToken);
        _jwtService.Setup(x => x.GenerateRefreshToken()).Returns(mockedRefreshToken);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(mockedAccessToken, response.AccessToken);
        Assert.Equal(mockedRefreshToken.Token, response.RefreshToken);
        Assert.Equal(mockedRefreshToken.ExpiryTime, response.RefreshTokenExpiryTime);
        _userRepository.Verify(x => x.GetRolesAsync(user), Times.Once);
    }
}