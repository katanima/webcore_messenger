using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Guilds.Dtos;
using webcore_backend.Features.Invites.Services;
using webcore_backend.Infrastructure.Http;

namespace webcore_backend.Features.Guilds.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GuildInviteController(IGuildInviteService _inviteService, IUserContextAccessor _userContext) : ControllerBase
{
    private readonly Guid _currentUserId = _userContext.Id();
    
    [HttpPost]
    public async Task<IActionResult> CreateInvitationAsync(CreateInvitationRequestDto request)
    {
        await _inviteService.CreateInvitationAsync(_currentUserId, request);
        return Ok();
    }
}