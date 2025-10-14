using System.IdentityModel.Tokens.Jwt;
using webcore_backend.Features.Auth.Dtos;

namespace webcore_backend.Features.Auth.Services;

public interface IAuthService
{
    Task<string> AuthUserAsync(AuthUserRequestDto request);
}