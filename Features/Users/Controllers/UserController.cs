using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Users.Dtos;
using webcore_backend.Features.Users.Services;

namespace webcore_backend.Features.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserRequestDto dto)
        => Ok(await _userService.RegisterUserAsync(dto));
    
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserFromBearerTokenAsync() 
        => Ok(await _userService.GetUserFromBearerTokenAsync());
}