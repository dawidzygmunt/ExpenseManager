using System.Net;
using System.Net.Http.Json;
using ExpensesManager.Api.DTO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ExpensesManager.Api.IntegrationTests.Controllers;

public class AuthControllerTest: IClassFixture<PostgresContainerFixture>
{
    private readonly HttpClient _client;
    public AuthControllerTest(PostgresContainerFixture postgresContainerFixture)
    {
        var factory = new CustomWebApplicationFactory(postgresContainerFixture);
        _client = factory.CreateClient();
        
    }

    [Fact]
    public async Task AuthController_RegisterThenLogin_ReturnsOk()
    {
        
        // Arrange
        var request = new RegisterRequest("test@test.com", "TestPassword123", "John", "Doe");

        // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
}