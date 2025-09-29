using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Guilds.Dtos;
using webcore_backend.Infrastructure.Http;


namespace webcore_backend.Features.Guilds.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GuildController(IGuildService _guildService, IUserContextAccessor _userContext) : ControllerBase
{
    private readonly Guid _currentUserId = _userContext.Id();
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateGuildAsync(CreateGuildRequestDto request)
        => Ok(await _guildService.CreateGuildAsync(_currentUserId, request));

    [HttpPatch("edit")]
    public async Task<IActionResult> EditGuildAsync(EditGuildRequestDto request)
    {
        await _guildService.EditGuildAsync(_currentUserId, request);
        return Ok();
    }
    
    [HttpPost("join/{guildId:guid}")]
    public async Task<IActionResult> JoinGuildAsync(Guid guildId)
    {
        await _guildService.LeaveGuildAsync(_currentUserId, guildId);
        return Ok();
    }

    [HttpPost("leave/{guildId:guid}")]
    public async Task<IActionResult> LeaveGuildAsync(Guid guildId)
    {
        await _guildService.LeaveGuildAsync(_currentUserId, guildId);
        return Ok();
    }
}