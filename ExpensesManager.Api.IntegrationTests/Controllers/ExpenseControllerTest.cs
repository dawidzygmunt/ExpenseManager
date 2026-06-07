using System.Net;
using System.Net.Http.Json;
using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Api.IntegrationTests.Controllers;

public class ExpenseControllerTest : IClassFixture<PostgresContainerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;

    public ExpenseControllerTest(PostgresContainerFixture postgresContainerFixture)
    {
        var factory = new CustomWebApplicationFactory(postgresContainerFixture);
        _client = factory.CreateClient();
    }

    public Task InitializeAsync() => _client.AuthenticateAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ExpenseController_AddExpense_ReturnsCreated()
    {
        // Arrange
        var command = new AddExpenseCommand(
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
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/expense", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ExpenseController_AddExpense_ReturnsExpenseWithCorrectData()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new AddExpenseCommand(
            250m,
            ExpenseType.Transport,
            "Train ticket",
            DateTime.UtcNow,
            PaymentMethod.BankTransfer,
            "Work trip",
            Currency.EUR,
            false,
            null,
            null,
            workspaceId,
            userId
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/expense", command);
        var body = await response.ReadApiDataAsync<ExpenseResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.Id);
        Assert.Equal(command.Amount, body.Amount);
        Assert.Equal(command.Description, body.Description);
        Assert.Equal(command.WorkspaceId, body.WorkspaceId);
        Assert.Equal(command.UserId, body.UserId);
    }

    [Fact]
    public async Task ExpenseController_AddExpense_ReturnsLocationHeader()
    {
        // Arrange
        var command = new AddExpenseCommand(
            50m,
            ExpenseType.Entertainment,
            "Cinema",
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

        // Act
        var response = await _client.PostAsJsonAsync("/api/expense", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task ExpenseController_GetExpenseById_AfterAdd_ReturnsOk()
    {
        // Arrange
        var command = new AddExpenseCommand(
            99m,
            ExpenseType.Food,
            "Groceries",
            DateTime.UtcNow,
            PaymentMethod.Card,
            null,
            Currency.PLN,
            false,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        var postResponse = await _client.PostAsJsonAsync("/api/expense", command);
        var created = await postResponse.ReadApiDataAsync<ExpenseResponse>();

        // Act
        var response = await _client.GetAsync($"/api/expense/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ExpenseController_GetExpenseById_AfterAdd_ReturnsCorrectExpense()
    {
        // Arrange
        var command = new AddExpenseCommand(
            300m,
            ExpenseType.Other,
            "Office supplies",
            DateTime.UtcNow,
            PaymentMethod.BankTransfer,
            "For home office",
            Currency.USD,
            false,
            null,
            null,
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        var postResponse = await _client.PostAsJsonAsync("/api/expense", command);
        var created = await postResponse.ReadApiDataAsync<ExpenseResponse>();

        // Act
        var response = await _client.GetAsync($"/api/expense/{created.Id}");
        var body = await response.ReadApiDataAsync<ExpenseResponse>();

        // Assert
        Assert.NotNull(body);
        Assert.Equal(created.Id, body.Id);
        Assert.Equal(command.Amount, body.Amount);
        Assert.Equal(command.Description, body.Description);
    }

    [Fact]
    public async Task ExpenseController_DeleteExpense_AfterAdd_ReturnsOk()
    {
        // Arrange
        var command = new AddExpenseCommand(
            15m,
            ExpenseType.Food,
            "Snack",
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

        var postResponse = await _client.PostAsJsonAsync("/api/expense", command);
        var created = await postResponse.ReadApiDataAsync<ExpenseResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/expense/{created.Id}");

        // Assert
        // ApiResponseFilter rewrites 204 NoContent into a wrapped 200 OK response.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ExpenseController_GetAllExpenses_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/expense");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}