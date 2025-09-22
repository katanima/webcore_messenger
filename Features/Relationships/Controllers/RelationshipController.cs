using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Friends.Services;

namespace webcore_backend.Features.Relationships.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelationshipController(IRelationshipService _relationshipService) : ControllerBase
{
    [HttpGet("friends")]
    public async Task<IActionResult> GetFriendListAsync()
        => Ok(await _relationshipService.GetFriendListByBearerTokenAsync());

    [HttpGet("requests")]
    public async Task<IActionResult> GetFriendRequestsAsync()
        => Ok(await _relationshipService.GetFriendRequestListByBearerTokenAsync());

    [HttpPost("request/{targetUserId:guid}")]
    public async Task<IActionResult> SendFriendRequestAsync(Guid targetUserId)
    {
        await _relationshipService.SendFriendRequestAsync(targetUserId);
        return Ok();
    }

    [HttpPost("accept/{senderUserId:guid}")]
    public async Task<IActionResult> AcceptFriendRequestAsync(Guid senderUserId)
    {
        await _relationshipService.AcceptFriendRequestAsync(senderUserId);
        return Ok();
    }

    [HttpPost("decline/{senderUserId:guid}")]
    public async Task<IActionResult> DeclineFriendRequestAsync(Guid senderUserId)
    {
        await _relationshipService.DeclineFriendRequestAsync(senderUserId);
        return Ok();
    }

    [HttpDelete("remove/{targetUserId:guid}")]
    public async Task<IActionResult> RemoveFriendAsync(Guid targetUserId)
    {
        await _relationshipService.RemoveFriendAsync(targetUserId);
        return Ok();
    }

    [HttpPost("block/{targetUserId:guid}")]
    public async Task<IActionResult> BlockUserAsync(Guid targetUserId)
    {
        await _relationshipService.BlockUserAsync(targetUserId);
        return Ok();
    }

    [HttpPost("unblock/{targetUserId:guid}")]
    public async Task<IActionResult> UnblockUserAsync(Guid targetUserId)
    {
        await _relationshipService.UnblockUserAsync(targetUserId);
        return Ok();
    }
}
