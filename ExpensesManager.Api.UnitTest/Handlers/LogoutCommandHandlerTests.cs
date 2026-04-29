using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using MediatR;
using Moq;

namespace ExpensesManager.Api.UnitTest.Handlers;

public class LogoutCommandHandlerTests
{
    private readonly Mock<IBlackListRepository> _blackListRepository = new();
    private readonly Mock<IJwtService> _jwtService = new();
    private readonly LogoutCommandHandler _sut;

    public LogoutCommandHandlerTests()
    {
        _sut = new LogoutCommandHandler(_blackListRepository.Object, _jwtService.Object);
    }

    [Fact]
    public async Task LogoutCommandHandler_Handle_WhenTokenAlreadyBlacklisted_ThrowsException()
    {
        // Arrange
        var request = new LogoutCommand("mockedAccessToken");
        var decoded = (jti: "mocked-jti", expiry: DateTime.UtcNow.AddMinutes(30));

        _jwtService.Setup(x => x.DecodeAccessToken(request.AccessToken)).Returns(decoded);
        _blackListRepository.Setup(x => x.IsBlacklistedAsync(decoded.jti)).ReturnsAsync(true);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.Handle(request, CancellationToken.None));
        Assert.Equal("Access Token is already on the black list", ex.Message);
        _blackListRepository.Verify(x => x.AddAsync(It.IsAny<BlackListedToken>()), Times.Never);
    }

    [Fact]
    public async Task LogoutCommandHandler_Handle_WhenTokenNotBlacklisted_AddsTokenToBlackList()
    {
        // Arrange
        var request = new LogoutCommand("mockedAccessToken");
        var decoded = (jti: "mocked-jti", expiry: DateTime.UtcNow.AddMinutes(30));

        _jwtService.Setup(x => x.DecodeAccessToken(request.AccessToken)).Returns(decoded);
        _blackListRepository.Setup(x => x.IsBlacklistedAsync(decoded.jti)).ReturnsAsync(false);
        _blackListRepository.Setup(x => x.AddAsync(It.IsAny<BlackListedToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _blackListRepository.Verify(x => x.AddAsync(It.Is<BlackListedToken>(t =>
            t.Jti == decoded.jti &&
            t.Expiry == decoded.expiry &&
            t.Id != Guid.Empty)), Times.Once);
    }

    [Fact]
    public async Task LogoutCommandHandler_Handle_AlwaysDecodesProvidedAccessToken()
    {
        // Arrange
        var request = new LogoutCommand("mockedAccessToken");
        var decoded = (jti: "mocked-jti", expiry: DateTime.UtcNow.AddMinutes(30));

        _jwtService.Setup(x => x.DecodeAccessToken(request.AccessToken)).Returns(decoded);
        _blackListRepository.Setup(x => x.IsBlacklistedAsync(decoded.jti)).ReturnsAsync(false);

        // Act
        await _sut.Handle(request, CancellationToken.None);

        // Assert
        _jwtService.Verify(x => x.DecodeAccessToken(request.AccessToken), Times.Once);
        _blackListRepository.Verify(x => x.IsBlacklistedAsync(decoded.jti), Times.Once);
    }
}