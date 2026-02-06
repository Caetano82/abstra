using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Abstra.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // Simple authentication - in production, validate against a user store
        // For demo purposes, accept any non-empty username and password
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        try
        {
            // Generate JWT token for any valid (non-empty) credentials
            var token = GenerateJwtToken(request.Username);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            return await Task.FromResult(new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt
            });
        }
        catch (Exception ex)
        {
            // Log error if token generation fails
            System.Diagnostics.Debug.WriteLine($"Error generating token: {ex.Message}");
            return null;
        }
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
        var issuer = jwtSettings["Issuer"] ?? "AbstraApi";
        var audience = jwtSettings["Audience"] ?? "AbstraApi";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "1440");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
