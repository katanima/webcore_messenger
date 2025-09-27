using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webcore_backend.Configurations;
using webcore_backend.Features.Auth.Dtos;
using webcore_backend.Features.Users.Entities;
using webcore_backend.Features.Users.Services;

namespace webcore_backend.Features.Auth.Services;

public class AuthService(IUserService _userService, IConfiguration _config, IOptions<JwtSettings> _options) : IAuthService
{
    private readonly JwtSettings _jwtSettings = _options.Value;
    
    public async Task<string> AuthUserAsync(AuthUserRequestDto request)
    {
        var user = !string.IsNullOrWhiteSpace(request.Email)
            ? await _userService.FindUserByEmailAsync(request.Email)
            : await _userService.FindUserByUsernameAsync(request.Username!)
              ?? throw new UnauthorizedAccessException("User not found");

        var isPasswordValid = await _userService.VerifyPasswordAsync(user, request.Password);
        
        return isPasswordValid ? GenerateJwtToken(user) : throw new UnauthorizedAccessException();
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
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}