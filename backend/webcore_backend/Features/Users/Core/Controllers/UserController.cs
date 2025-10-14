using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Users.Core.Dtos;
using webcore_backend.Features.Users.Core.Services;
using webcore_backend.Infrastructure.Http;

namespace webcore_backend.Features.Users.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "User")]
public class UserController(IUserService _userService, IUserContextAccessor _userContext) : ControllerBase
{
    private Guid UserId => _userContext.Id();
    
    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="dto">New user data</param>
    /// <returns>New user ID if succeed</returns>
    [HttpPost]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserRequestDto dto)
        => Ok(await _userService.RegisterUserAsync(dto));
    
    
    /// <summary>
    /// Get logged user information
    /// </summary>
    /// <returns>User data</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserByIdAsync() 
        => Ok(await _userService.GetUserAsync(UserId));
}