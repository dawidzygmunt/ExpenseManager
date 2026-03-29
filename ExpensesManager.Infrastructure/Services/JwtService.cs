using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesManager.Infrastructure.Services;

public class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
{
    public string GenerateAccessToken(User user, List<string> roles)
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

        // Add roles as claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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
}