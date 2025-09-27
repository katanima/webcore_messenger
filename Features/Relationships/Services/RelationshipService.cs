using Microsoft.EntityFrameworkCore;
using webcore_backend.Extensions;
using webcore_backend.Features.Friends.Dtos;
using webcore_backend.Features.Friends.Entity;
using webcore_backend.Features.Friends.Models;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Dtos;

namespace webcore_backend.Features.Friends.Services;

public class RelationshipService(AppDbContext _dbContext) : IRelationshipService
{
    private async Task<IEnumerable<UserGeneralInformationsDto>> GetUserInformationsByRelationshipStatusAsync(Guid currentUserId, RelationshipStatus status)
    {
        var relationships = await _dbContext.Relationship
            .Where(r => r.Status == status &&
                        (r.SenderId == currentUserId || r.ReceiverId == currentUserId))
            .ToListAsync();

        var otherUserIds = relationships
            .Select(r => r.SenderId == currentUserId ? r.ReceiverId : r.SenderId)
            .ToList();

        var users = await _dbContext.User
            .Where(u => otherUserIds.Contains(u.Id))
            .Select(u => new UserGeneralInformationsDto(u.Username))
            .ToListAsync();

        return users;
    }

    public async Task<GetFriendListResponseDto> GetFriendListAsync(Guid currentUserId)
    {
        var friends = await GetUserInformationsByRelationshipStatusAsync(currentUserId, RelationshipStatus.Accepted);
        return new GetFriendListResponseDto(friends);
    }

    public async Task<GetFriendRequestListResponseDto> GetFriendRequestListAsync(Guid currentUserId)
    {
        var requests = await GetUserInformationsByRelationshipStatusAsync(currentUserId, RelationshipStatus.Pending);
        return new GetFriendRequestListResponseDto(requests);
    }

    public async Task SendFriendRequestAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = new RelationshipEntity
        {
            SenderId = currentUserId,
            ReceiverId = targetUserId,
        };
        relationship.SetStatus(RelationshipStatus.Pending);

        await _dbContext.AddAsync(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AcceptFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var request = await _dbContext.Relationship
            .SingleOrDefaultAsync(r => r.Status == RelationshipStatus.Pending &&
                                       r.SenderId == senderUserId &&
                                       r.ReceiverId == currentUserId)
            ?? throw new InvalidOperationException("Friend request not found");

        request.SetStatus(RelationshipStatus.Accepted);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeclineFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var request = await _dbContext.Relationship
            .SingleOrDefaultAsync(r => r.Status == RelationshipStatus.Pending &&
                                       r.SenderId == senderUserId &&
                                       r.ReceiverId == currentUserId)
            ?? throw new InvalidOperationException("Friend request not found");

        _dbContext.Remove(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendAsync(Guid currentUserId, Guid targetUserId)
    {
        var friend = await _dbContext.Relationship
            .SingleOrDefaultAsync(r => r.Status == RelationshipStatus.Accepted &&
                                       ((r.SenderId == currentUserId && r.ReceiverId == targetUserId) ||
                                        (r.SenderId == targetUserId && r.ReceiverId == currentUserId)))
            ?? throw new InvalidOperationException("Friend not found");

        _dbContext.Remove(friend);
        await _dbContext.SaveChangesAsync();
    }

    public async Task BlockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await _dbContext.Relationship
            .SingleOrDefaultAsync(r => (r.SenderId == currentUserId && r.ReceiverId == targetUserId) ||
                                       (r.SenderId == targetUserId && r.ReceiverId == currentUserId));

        if (relationship == null)
        {
            relationship = new RelationshipEntity
            {
                SenderId = currentUserId,
                ReceiverId = targetUserId
            };
            _dbContext.Add(relationship);
        }

        relationship.SetStatus(RelationshipStatus.Blocked);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await _dbContext.Relationship
            .SingleOrDefaultAsync(r => (r.SenderId == currentUserId && r.ReceiverId == targetUserId) ||
                                       (r.SenderId == targetUserId && r.ReceiverId == currentUserId) &&
                                       r.Status == RelationshipStatus.Blocked) 
                           ?? throw new InvalidOperationException("Blocked user not found");

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }
}
