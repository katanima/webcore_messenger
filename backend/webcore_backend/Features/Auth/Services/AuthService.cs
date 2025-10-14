using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webcore_backend.Configurations;
using webcore_backend.Features.Auth.Dtos;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Features.Auth.Services;

public class AuthService(AppDbContext _dbContext, IConfiguration _config, IOptions<JwtSettings> _options) : IAuthService
{
    private readonly JwtSettings _jwtSettings = _options.Value;
    private readonly PasswordHasher<UserEntity> _hasher = new();
    
    public async Task<string> AuthUserAsync(AuthUserRequestDto request)
    {
        var user = !string.IsNullOrWhiteSpace(request.Email)
            ? await _dbContext.User.FirstOrDefaultAsync(u => u.Email == request.Email)
            : await _dbContext.User.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            throw new UnauthorizedAccessException("User not found");
        
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        
        return result == PasswordVerificationResult.Success
            ? GenerateJwtToken(user)
            : throw new UnauthorizedAccessException();
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}