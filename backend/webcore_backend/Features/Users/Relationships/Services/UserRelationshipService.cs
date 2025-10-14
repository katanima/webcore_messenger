using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Users.Relationships.Dtos;
using webcore_backend.Features.Users.Relationships.Entities;
using webcore_backend.Features.Users.Relationships.Models;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Dtos;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Features.Users.Relationships.Services;

public class UserRelationshipService(AppDbContext _dbContext) : IUserRelationshipService
{
    private record SortedPair(Guid UserAId, Guid UserBId);
    private static SortedPair SortIds(Guid userAId, Guid userBId)
    {
        var ordered = new[] { userAId, userBId }.OrderBy(id => id).ToArray();
        return new SortedPair(ordered[0], ordered[1]);
    }

    private async Task<UserRelationshipEntity> RequireRelationshipByStatus(Guid idA, Guid idB, UserRelationshipStatus status)
    {
        var sortedIds = SortIds(idA, idB);
        
        return await _dbContext.Relationship
                   .SingleOrDefaultAsync(r => r.Status == status && r.UserAId == sortedIds.UserAId && r.UserBId == sortedIds.UserBId) 
               ?? throw new InvalidOperationException("Friend request not found");
    }

    private async Task<UserRelationshipEntity?> FindRelationship(Guid idA, Guid idB)
    {
        var sortedIds = SortIds(idA, idB);
        
        return await _dbContext.Relationship
            .SingleOrDefaultAsync(r => (r.UserAId == sortedIds.UserAId && r.UserBId == sortedIds.UserBId));
    }
    
    private async Task<IEnumerable<UserGeneralInformationsDto>> GetUserInformationsByRelationshipStatusAsync(Guid currentUserId, UserRelationshipStatus status)
    {
        var relationships = await _dbContext.Relationship
            .Where(r => r.Status == status &&
                        (r.UserAId == currentUserId || r.UserBId == currentUserId))
            .ToListAsync();

        var otherUserIds = relationships
            .Select(r => r.UserAId == currentUserId ? r.UserBId : r.UserAId)
            .ToList();

        var users = await _dbContext.User
            .Where(u => otherUserIds.Contains(u.Id))
            .Select(u => new UserGeneralInformationsDto(u.Username))
            .ToListAsync();

        return users;
    }

    public async Task<GetFriendListResponseDto> GetFriendListAsync(Guid currentUserId)
    {
        var friends = await GetUserInformationsByRelationshipStatusAsync(currentUserId, UserRelationshipStatus.Accepted);
        return new GetFriendListResponseDto(friends);
    }

    public async Task<GetFriendRequestListResponseDto> GetFriendRequestListAsync(Guid currentUserId)
    {
        var requests = await GetUserInformationsByRelationshipStatusAsync(currentUserId, UserRelationshipStatus.Pending);
        return new GetFriendRequestListResponseDto(requests);
    }

    private async Task<UserEntity> RequireUser(Guid userId)
        => await _dbContext.User.FindAsync(userId)
           ?? throw new InvalidOperationException($"User with ID {userId} not found");

    public async Task SendFriendRequestAsync(Guid currentUserId, Guid targetUserId)
    {
        var sortedIds = SortIds(currentUserId, targetUserId);

        var relationship = await _dbContext.Relationship.FindAsync(sortedIds.UserAId, sortedIds.UserBId);
        if (relationship != null)
        {
            throw relationship.Status switch
            {
                UserRelationshipStatus.Accepted => new InvalidOperationException("Target user is already a friend"),
                UserRelationshipStatus.Pending => new InvalidOperationException("Friend request already exists"),
                UserRelationshipStatus.Blocked => new InvalidOperationException("Target user is blocked"),
                _ => new InvalidOperationException("Relationship exists with unknown status")
            };
        }
        
        var userA = await RequireUser(sortedIds.UserAId);
        var userB = await RequireUser(sortedIds.UserBId);
        
        relationship = new UserRelationshipEntity
        { 
            UserAId = sortedIds.UserAId,
            UserBId = sortedIds.UserBId,
            UserA = userA,
            UserB = userB,
        };
        relationship.SetStatus(UserRelationshipStatus.Pending);

        await _dbContext.AddAsync(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AcceptFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, senderUserId, UserRelationshipStatus.Pending);

        relationship.SetStatus(UserRelationshipStatus.Accepted);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeclineFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, senderUserId, UserRelationshipStatus.Pending);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendAsync(Guid currentUserId, Guid friendUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, friendUserId, UserRelationshipStatus.Accepted);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task BlockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await FindRelationship(currentUserId, targetUserId);

        if (relationship == null)
        {
            var sortedIds = SortIds(currentUserId, targetUserId);
            var userA = await RequireUser(sortedIds.UserAId);
            var userB = await RequireUser(sortedIds.UserBId);
            
            relationship = new UserRelationshipEntity
            {
                UserAId = sortedIds.UserAId,
                UserBId = sortedIds.UserBId,
                UserA = userA,
                UserB = userB
            };
            _dbContext.Add(relationship);
        }

        relationship.SetStatus(UserRelationshipStatus.Blocked);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, targetUserId, UserRelationshipStatus.Blocked);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }
}
