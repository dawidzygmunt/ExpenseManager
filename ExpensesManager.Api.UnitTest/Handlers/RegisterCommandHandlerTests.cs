using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using Moq;

namespace ExpensesManager.Api.UnitTest.Handlers;

public class RegisterCommandHandlerTests
{
    private readonly RegisterCommandHandler _sut;
    private readonly Mock<IUserRepository> _userRepository = new();


    public RegisterCommandHandlerTests()
    {
        _sut = new RegisterCommandHandler(_userRepository.Object);
    }


    [Fact]
    public async Task
        RegisterCommandHandler_Handle_WhenUserDoesNotExist_ReturnsSuccess()
    {
        // Arrange
        var request = new RegisterCommand("admin@test.com", "testPassword123",
            "John", "Doe");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);
        _userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), request.Password))
            .ReturnsAsync(user);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(user.Id, response.UserDto.Id);
        Assert.Equal(user.Email, response.UserDto.Email);
        Assert.Equal(user.FirstName, response.UserDto.FirstName);
        Assert.Equal(user.LastName, response.UserDto.LastName);

        _userRepository.Verify(
            x => x.AddAsync(It.IsAny<User>(), request.Password), Times.Once);
        _userRepository.Verify(
            x => x.GetByEmailAsync(request.Email), Times.Once);
    }

    [Fact]
    public async Task
        RegisterCommandHandler_Handle_WhenEmailAlreadyExists_ThrowsException()
    {
        // Arrange
        var request = new RegisterCommand("admin@test.com", "testPassword123",
            "John", "Doe");
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email,
            FirstName = "Existing",
            LastName = "User"
        };

        _userRepository.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<Exception>(() => _sut.Handle(request, CancellationToken.None));

        Assert.Equal("User with this email already exists", exception.Message);

        _userRepository.Verify(
            x => x.GetByEmailAsync(request.Email), Times.Once);
        _userRepository.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<string>()),
            Times.Never);
    }
}