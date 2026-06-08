using System.Net.Http.Headers;
using System.Net.Http.Json;
using ExpensesManager.Api.DTO;

namespace ExpensesManager.Api.IntegrationTests;

public static class AuthenticationHelper
{
    /// <summary>
    /// Registers a fresh user, logs in and sets the resulting JWT as the
    /// default Bearer token on the client so subsequent requests pass [Authorize].
    /// </summary>
    public static async Task AuthenticateAsync(this HttpClient client)
    {
        var email = $"user-{Guid.NewGuid():N}@test.com";
        const string password = "TestPassword123";

        var register = new RegisterRequest(email, password, "Test", "User");
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", register);
        registerResponse.EnsureSuccessStatusCode();

        var login = new LoginRequest(email, password);
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", login);
        loginResponse.EnsureSuccessStatusCode();

        var body = await loginResponse.ReadApiDataAsync<LoginResult>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", body.Token);
    }

    // AuthController returns { Token, UserDto }, wrapped by ApiResponseFilter into { success, data, ... }.
    private sealed record LoginResult(string Token);
}
