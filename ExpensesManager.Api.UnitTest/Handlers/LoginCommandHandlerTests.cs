using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Exceptions;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using Moq;

namespace ExpensesManager.Api.UnitTest.Handlers;

public class LoginCommandHandlerTests
{
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly LoginCommandHandler _sut;
    private readonly Mock<IUserRepository> _userRepository = new();

    public LoginCommandHandlerTests()
    {
        _sut = new LoginCommandHandler(_jwtService.Object, _userRepository.Object,
            _refreshTokenRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenEmailIsEmpty_ThrowArgumentException()
    {
        // Arrange
        var request = new LoginCommand("", "password");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenPasswordIsEmpty_ThrowArgumentException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenUserDoesNotExist_ThrowUnauthorizedException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        _userRepository
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task LoginCommandHandler_Handle_WhenInvalidPassword_ThrowUnauthorizedException()
    {
        // Arrange
        var request = new LoginCommand("test@test.com", "password");
        var user = new User { Email = request.Email };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _userRepository.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.Handle(request, CancellationToken.None));
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
            Token = "mockedRefreshToken",
            ExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _userRepository.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _userRepository.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);

        _jwtService.Setup(x => x.GenerateAccessToken(user, roles)).Returns(mockedAccessToken);
        _jwtService.Setup(x => x.GenerateRefreshToken()).Returns(mockedRefreshToken);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(mockedAccessToken, response.Token);
        Assert.Equal(mockedRefreshToken.Token, response.RefreshToken);
        Assert.Equal(mockedRefreshToken.ExpiryTime, response.RefreshTokenExpiryTime);
        Assert.Equal(user.Id, response.UserDto.Id);
        Assert.Equal(user.Email, response.UserDto.Email);
        Assert.Equal(user.FirstName, response.UserDto.FirstName);
        Assert.Equal(user.LastName, response.UserDto.LastName);

        _userRepository.Verify(x => x.CheckPasswordAsync(user, request.Password), Times.Once);
        _userRepository.Verify(x => x.GetRolesAsync(user), Times.Once);
    }
}