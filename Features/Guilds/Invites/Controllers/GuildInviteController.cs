using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Guilds.Invites.Dtos;
using webcore_backend.Features.Guilds.Invites.Services;
using webcore_backend.Infrastructure.Http;

namespace webcore_backend.Features.Guilds.Invites.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "Guild")]
[Route("[controller]")]
[Authorize]
public class GuildInviteController(IGuildInviteService _inviteService, IUserContextAccessor _userContext) : ControllerBase
{
    private readonly Guid _currentUserId = _userContext.Id();
    
    /// <summary>
    /// Creates a new invitation using the specified request data.
    /// </summary>
    /// <param name="request">An object containing the details required to create the invitation. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the invitation creation operation.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateInvitationAsync(CreateInvitationRequestDto request)
    {
        await _inviteService.CreateInvitationAsync(_currentUserId, request);
        return Ok();
    }
}