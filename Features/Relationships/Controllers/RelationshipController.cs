using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcore_backend.Features.Friends.Services;

namespace webcore_backend.Features.Relationships.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelationshipController(IRelationshipService _relationshipService) : ControllerBase
{
    /// <summary>
    /// Get user friend list
    /// </summary>
    /// <returns>Friend list</returns>
    [HttpGet("friends")]
    public async Task<IActionResult> GetFriendListAsync()
        => Ok(await _relationshipService.GetFriendListByBearerTokenAsync());

    /// <summary>
    /// Get requests from other users to become friend
    /// </summary>
    /// <returns>Friend request list</returns>
    [HttpGet("requests")]
    public async Task<IActionResult> GetFriendRequestsAsync()
        => Ok(await _relationshipService.GetFriendRequestListByBearerTokenAsync());

    /// <summary>
    /// Send friend request to somebody
    /// </summary>
    /// <param name="targetUserId">Any user ID</param>
    /// <returns>Confirmation of operation</returns>
    [HttpPost("request/{targetUserId:guid}")]
    public async Task<IActionResult> SendFriendRequestAsync(Guid targetUserId)
    {
        await _relationshipService.SendFriendRequestAsync(targetUserId);
        return Ok();
    }

    /// <summary>
    /// Accept somebody's friend request
    /// </summary>
    /// <param name="senderUserId">User ID that sent the request</param>
    /// <returns>Confirmation of operation</returns>
    [HttpPost("accept/{senderUserId:guid}")]
    public async Task<IActionResult> AcceptFriendRequestAsync(Guid senderUserId)
    {
        await _relationshipService.AcceptFriendRequestAsync(senderUserId);
        return Ok();
    }

    /// <summary>
    /// Reject somebody's friend request
    /// </summary>
    /// <param name="senderUserId">User ID that sent the request</param>
    /// <returns>Confirmation of operation</returns>
    [HttpPost("decline/{senderUserId:guid}")]
    public async Task<IActionResult> DeclineFriendRequestAsync(Guid senderUserId)
    {
        await _relationshipService.DeclineFriendRequestAsync(senderUserId);
        return Ok();
    }

    /// <summary>
    /// Remove user from friend list
    /// </summary>
    /// <param name="targetUserId">User ID from friend list</param>
    /// <returns>Confirmation of operation</returns>
    [HttpDelete("remove/{targetUserId:guid}")]
    public async Task<IActionResult> RemoveFriendAsync(Guid targetUserId)
    {
        await _relationshipService.RemoveFriendAsync(targetUserId);
        return Ok();
    }

    /// <summary>
    /// Block any user
    /// </summary>
    /// <param name="targetUserId">Any user ID</param>
    /// <returns>Confirmation of operation</returns>
    [HttpPost("block/{targetUserId:guid}")]
    public async Task<IActionResult> BlockUserAsync(Guid targetUserId)
    {
        await _relationshipService.BlockUserAsync(targetUserId);
        return Ok();
    }

    /// <summary>
    /// Unblocked blocked user
    /// </summary>
    /// <param name="targetUserId">Blocked user ID</param>
    /// <returns>Confirmation of operation</returns>
    [HttpPost("unblock/{targetUserId:guid}")]
    public async Task<IActionResult> UnblockUserAsync(Guid targetUserId)
    {
        await _relationshipService.UnblockUserAsync(targetUserId);
        return Ok();
    }
}
