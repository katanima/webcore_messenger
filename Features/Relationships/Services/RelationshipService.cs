using Microsoft.EntityFrameworkCore;
using webcore_backend.Extensions;
using webcore_backend.Features.Friends.Dtos;
using webcore_backend.Features.Friends.Entity;
using webcore_backend.Features.Friends.Models;
using webcore_backend.Features.Users.Services;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Dtos;

namespace webcore_backend.Features.Friends.Services;

public class RelationshipService(AppDbContext _dbContext, IUserService _userService) : IRelationshipService
{
    private record SortedPair(Guid UserAId, Guid UserBId);
    private static SortedPair SortIds(Guid userAId, Guid userBId)
    {
        var ordered = new[] { userAId, userBId }.OrderBy(id => id).ToArray();
        return new SortedPair(ordered[0], ordered[1]);
    }

    private async Task<RelationshipEntity> RequireRelationshipByStatus(Guid idA, Guid idB, RelationshipStatus status)
    {
        var sortedIds = SortIds(idA, idB);
        
        return await _dbContext.Relationship
                   .SingleOrDefaultAsync(r => r.Status == RelationshipStatus.Pending && 
                                              r.UserAId == sortedIds.UserAId && r.UserBId == sortedIds.UserBId) 
               ?? throw new InvalidOperationException("Friend request not found");
    }

    private async Task<RelationshipEntity?> FindRelationship(Guid idA, Guid idB)
    {
        var sortedIds = SortIds(idA, idB);
        
        return await _dbContext.Relationship
            .SingleOrDefaultAsync(r => (r.UserAId == sortedIds.UserAId && r.UserBId == sortedIds.UserBId));
    }
    
    private async Task<IEnumerable<UserGeneralInformationsDto>> GetUserInformationsByRelationshipStatusAsync(Guid currentUserId, RelationshipStatus status)
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
        var sortedIds = SortIds(currentUserId, targetUserId);
        
        var relationship = new RelationshipEntity
        { 
            UserAId = sortedIds.UserAId,
            UserBId = sortedIds.UserBId,
            UserA = await _userService.RequireUserByIdAsync(sortedIds.UserAId),
            UserB = await _userService.RequireUserByIdAsync(sortedIds.UserBId)
        };
        relationship.SetStatus(RelationshipStatus.Pending);

        await _dbContext.AddAsync(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AcceptFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, senderUserId, RelationshipStatus.Pending);

        relationship.SetStatus(RelationshipStatus.Accepted);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeclineFriendRequestAsync(Guid currentUserId, Guid senderUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, senderUserId, RelationshipStatus.Pending);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendAsync(Guid currentUserId, Guid friendUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, friendUserId, RelationshipStatus.Accepted);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task BlockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await FindRelationship(currentUserId, targetUserId);

        if (relationship == null)
        {
            var sortedIds = SortIds(currentUserId, targetUserId);
            
            relationship = new RelationshipEntity
            {
                UserAId = sortedIds.UserAId,
                UserBId = sortedIds.UserBId,
                UserA = await _userService.RequireUserByIdAsync(sortedIds.UserAId),
                UserB = await _userService.RequireUserByIdAsync(sortedIds.UserBId)
            };
            _dbContext.Add(relationship);
        }

        relationship.SetStatus(RelationshipStatus.Blocked);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(Guid currentUserId, Guid targetUserId)
    {
        var relationship = await RequireRelationshipByStatus(currentUserId, targetUserId, RelationshipStatus.Blocked);

        _dbContext.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }
}
