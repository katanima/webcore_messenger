using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Users.Relationships.Services;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Tests.Features.Users.Relationships.Services;

public class UserRelationshipServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task SendFriendRequestAsync_Should_Create_Relationship()
    {
        var context = GetDbContext();
        var user1 = new UserEntity { Username = "A", Email = "a@test.com", PasswordHash = "hash" };
        var user2 = new UserEntity { Username = "B", Email = "b@test.com", PasswordHash = "hash" };
        context.User.AddRange(user1, user2);
        await context.SaveChangesAsync();

        var service = new UserRelationshipService(context);

        await service.SendFriendRequestAsync(user1.Id, user2.Id);

        var rel = await context.Relationship.FirstOrDefaultAsync();
        Assert.NotNull(rel);
        Assert.Equal(user1.Id, rel!.UserAId == user1.Id ? rel.UserAId : rel.UserBId);
        Assert.Equal("Pending", rel.Status.ToString());
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_Should_Change_Status()
    {
        var context = GetDbContext();
        var user1 = new UserEntity { Username = "A", Email = "a@test.com", PasswordHash = "hash" };
        var user2 = new UserEntity { Username = "B", Email = "b@test.com", PasswordHash = "hash" };
        context.User.AddRange(user1, user2);
        await context.SaveChangesAsync();

        var service = new UserRelationshipService(context);
        await service.SendFriendRequestAsync(user1.Id, user2.Id);

        await service.AcceptFriendRequestAsync(user2.Id, user1.Id);

        var rel = await context.Relationship.FirstAsync();
        Assert.Equal("Accepted", rel.Status.ToString());
    }
}