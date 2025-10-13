using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Guilds.Core.Dtos;
using webcore_backend.Features.Guilds.Core.Services;
using webcore_backend.Infrastructure.Http;

namespace webcore_backend.Features.Guilds.Core.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "Guild")]
public class GuildController(IGuildService _guildService, IUserContextAccessor _userContext) : ControllerBase
{
    private readonly Guid _currentUserId = _userContext.Id();
    
    /// <summary>
    /// Creates a new guild using the specified request data.
    /// </summary>
    /// <param name="request">An object containing the details required to create the guild. Must not be null.</param>
    /// <returns>An IActionResult representing the result of the guild creation operation. Returns a success response with the
    /// created guild information if successful.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateGuildAsync(CreateGuildRequestDto request)
        => Ok(await _guildService.CreateGuildAsync(_currentUserId, request));

    /// <summary>
    /// Updates the details of an existing guild using the provided request data.
    /// </summary>
    /// <param name="request">An object containing the updated guild information. Must not be null.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    [HttpPatch("edit")]
    public async Task<IActionResult> EditGuildAsync(EditGuildRequestDto request)
    {
        await _guildService.EditGuildAsync(_currentUserId, request);
        return Ok();
    }
    
    /// <summary>
    /// Attempts to join the specified guild on behalf of the current user.
    /// </summary>
    /// <param name="guildId">The unique identifier of the guild to join.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the join operation.</returns>
    [HttpPost("join/{guildId:guid}")]
    public async Task<IActionResult> JoinGuildAsync(Guid guildId)
    {
        await _guildService.LeaveGuildAsync(_currentUserId, guildId);
        return Ok();
    }

    /// <summary>
    /// Removes the current user from the specified guild.
    /// </summary>
    /// <param name="guildId">The unique identifier of the guild to leave.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the leave operation.</returns>
    [HttpPost("leave/{guildId:guid}")]
    public async Task<IActionResult> LeaveGuildAsync(Guid guildId)
    {
        await _guildService.LeaveGuildAsync(_currentUserId, guildId);
        return Ok();
    }
}