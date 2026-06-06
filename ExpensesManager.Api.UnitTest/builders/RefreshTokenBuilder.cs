using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Api.UnitTest.builders;

public class RefreshTokenBuilder
{
    private DateTime _expiryTime = DateTime.UtcNow.AddMinutes(5);
    private string _token = "mockedRefreshToken";

    public RefreshTokenBuilder WithToken(string token)
    {
        _token = token;
        return this;
    }

    public RefreshTokenBuilder WithExpiryTime(DateTime expiryTime)
    {
        _expiryTime = expiryTime;
        return this;
    }

    public RefreshTokenBuilder ExpiredToken()
    {
        _expiryTime = DateTime.UtcNow.AddMinutes(-5);
        return this;
    }

    public RefreshToken Build()
    {
        return new RefreshToken
        {
            Token = _token,
            ExpiryTime = _expiryTime
        };
    }
}