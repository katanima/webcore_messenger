using webcore_backend.Features.Friends.Dtos;

namespace webcore_backend.Features.Friends.Services;

public interface IRelationshipService
{
    public Task<GetFriendListResponseDto> GetFriendListByBearerTokenAsync();
    public Task<GetFriendRequestListResponseDto> GetFriendRequestListByBearerTokenAsync();
    public Task SendFriendRequestAsync(Guid targetUserId);
    public Task AcceptFriendRequestAsync(Guid senderUserId);
    public Task DeclineFriendRequestAsync(Guid senderUserId);
    public Task RemoveFriendAsync(Guid targetUserId);
    public Task BlockUserAsync(Guid targetUserId);
    public Task UnblockUserAsync(Guid targetUserId);
}