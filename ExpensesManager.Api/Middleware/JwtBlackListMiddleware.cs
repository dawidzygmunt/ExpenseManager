using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Interfaces;

namespace ExpensesManager.Api.Middleware;

public class JwtBlacklistMiddleware(RequestDelegate next)
{
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";

    public async Task InvokeAsync(
        HttpContext context,
        IBlackListRepository blackListRepository,
        IJwtService jwtService)
    {
        if (!TryGetAccessToken(context, out var accessToken))
        {
            await next(context);
            return;
        }

        string jti;
        try
        {
            jti = jwtService.DecodeAccessToken(accessToken).jti;
        }
        catch
        {
            await WriteUnauthorizedAsync(context, "Invalid access token");
            return;
        }

        if (await blackListRepository.IsBlacklistedAsync(jti))
        {
            await WriteUnauthorizedAsync(context, "Access token has been revoked");
            return;
        }

        await next(context);
    }

    private static bool TryGetAccessToken(HttpContext context, out string accessToken)
    {
        accessToken = string.Empty;

        if (!context.Request.Headers.TryGetValue(AuthorizationHeader, out var header))
            return false;

        var headerValue = header.ToString();
        if (string.IsNullOrWhiteSpace(headerValue) ||
            !headerValue.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            return false;

        accessToken = headerValue[BearerPrefix.Length..].Trim();
        return !string.IsNullOrWhiteSpace(accessToken);
    }

    private static Task WriteUnauthorizedAsync(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(new { message });
    }
}