using webcore_backend.Features.Users.Relationships.Dtos;

namespace webcore_backend.Features.Users.Relationships.Services;

public interface IUserRelationshipService
{
    public Task<GetFriendListResponseDto> GetFriendListAsync(Guid currentUserId);
    public Task<GetFriendRequestListResponseDto> GetFriendRequestListAsync(Guid currentUserId);
    public Task SendFriendRequestAsync(Guid currentUserId, Guid targetUserId);
    public Task AcceptFriendRequestAsync(Guid currentUserId, Guid senderUserId);
    public Task DeclineFriendRequestAsync(Guid currentUserId, Guid senderUserId);
    public Task RemoveFriendAsync(Guid currentUserId, Guid targetUserId);
    public Task BlockUserAsync(Guid currentUserId, Guid targetUserId);
    public Task UnblockUserAsync(Guid currentUserId, Guid targetUserId);
}