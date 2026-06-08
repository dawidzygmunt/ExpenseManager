using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Factories;
using ExpensesManager.Application.Handlers;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using Moq;

namespace ExpensesManager.Api.UnitTest.Handlers;

public class AddExpenseCommandHandlerTests
{
    private readonly Mock<IExpenseRepository> _expenseRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly AddExpenseCommandHandler _sut;

    public AddExpenseCommandHandlerTests()
    {
        _sut = new AddExpenseCommandHandler(
            _expenseRepository.Object,
            _unitOfWork.Object,
            new ExpenseFactory());
    }

    [Fact]
    public async Task AddExpenseCommandHandler_Handle_WhenValidCommand_ReturnsExpenseResponse()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new AddExpenseCommand(
            100m,
            ExpenseType.Food,
            "Lunch",
            DateTime.UtcNow,
            PaymentMethod.Card,
            null,
            Currency.PLN,
            false,
            null,
            null,
            workspaceId,
            userId
        );

        _expenseRepository
            .Setup(x => x.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(request.Amount, response.Amount);
        Assert.Equal(request.Type, response.Type);
        Assert.Equal(request.Description, response.Description);
        Assert.Equal(request.WorkspaceId, response.WorkspaceId);
        Assert.Equal(request.UserId, response.UserId);
        _expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Once);
    }

    [Fact]
    public async Task AddExpenseCommandHandler_Handle_WhenValidCommand_AssignsNewId()
    {
        // Arrange
        var request = new AddExpenseCommand(
            50m,
            ExpenseType.Transport,
            "Bus ticket",
            DateTime.UtcNow,
            PaymentMethod.Cash,
            null,
            Currency.EUR,
            false,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _expenseRepository
            .Setup(x => x.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    [Fact]
    public async Task AddExpenseCommandHandler_Handle_WhenPeriodicExpense_SetsPeriodAndStartDate()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var request = new AddExpenseCommand(
            200m,
            ExpenseType.Other,
            "Subscription",
            DateTime.UtcNow,
            PaymentMethod.Card,
            "Monthly plan",
            Currency.USD,
            true,
            30,
            startDate,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _expenseRepository
            .Setup(x => x.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.IsPeriodic);
        Assert.Equal(30, response.Period);
        Assert.Equal(startDate, response.StartDate);
        _expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Once);
    }

    [Fact]
    public async Task AddExpenseCommandHandler_Handle_WhenValidCommand_SetsCreatedAtToUtcNow()
    {
        // Arrange
        var before = DateTime.UtcNow;
        var request = new AddExpenseCommand(
            75m,
            ExpenseType.Entertainment,
            "Cinema",
            DateTime.UtcNow,
            PaymentMethod.BankTransfer,
            null,
            Currency.PLN,
            false,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _expenseRepository
            .Setup(x => x.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.CreatedAt >= before);
        Assert.True(response.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task AddExpenseCommandHandler_Handle_WhenCalled_CallsRepositoryExactlyOnce()
    {
        // Arrange
        var request = new AddExpenseCommand(
            10m,
            ExpenseType.Food,
            "Coffee",
            DateTime.UtcNow,
            PaymentMethod.Cash,
            null,
            Currency.PLN,
            false,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        _expenseRepository
            .Setup(x => x.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        await _sut.Handle(request, CancellationToken.None);

        // Assert
        _expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Once);
    }
}