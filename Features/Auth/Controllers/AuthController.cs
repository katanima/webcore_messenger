using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Auth.Dtos;
using webcore_backend.Features.Auth.Services;

namespace webcore_backend.Features.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AuthUserRequestDto dto)
    {
        var token = await _authService.AuthUserAsync(dto);
        return Ok(new { Token = token });
    }
}