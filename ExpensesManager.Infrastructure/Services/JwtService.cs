using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Interfaces;
using ExpensesManager.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesManager.Infrastructure.Services;

public class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
{
    public string GenerateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Value.SecretKey);

        // Generate unique JWT ID (jti) for token blacklisting
        var jti = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, jti) // JWT ID for blacklisting
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.Value.AccessTokenExpirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte [128];
        var refreshTokenExpiresDate = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationDays);

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                ExpiryTime = refreshTokenExpiresDate,
                Token = Convert.ToBase64String(randomNumber),
            };
        }
    }
    

    public (string jti, DateTime expiry) DecodeAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(accessToken);

        var jti = token.Claims.First(t => t.Type == JwtRegisteredClaimNames.Jti).Value;
        var expiry = token.ValidTo;

        return (jti, expiry);
    }
}