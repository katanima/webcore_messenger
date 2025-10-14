using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Users.Core.Dtos;
using webcore_backend.Models;

namespace webcore_backend.Tests.Features.Users.Core.Services;
using webcore_backend.Features.Users.Core.Services;

public class UserServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unikalna baza dla każdego testu
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task RegisterUserAsync_Should_Create_User()
    {
        // Arrange
        var context = GetDbContext();
        var service = new UserService(context);
        var dto = new RegisterUserRequestDto("testuser", "test@example.com", "P@ssw0rd");

        // Act
        var userId = await service.RegisterUserAsync(dto);

        // Assert
        var user = await context.User.FindAsync(userId);
        Assert.NotNull(user);
        Assert.Equal(dto.Username, user!.Username);
        Assert.Equal(dto.Email, user.Email);
    }

    [Fact]
    public async Task GetUserAsync_Should_Return_User()
    {
        var context = GetDbContext();
        var service = new UserService(context);

        var dto = new RegisterUserRequestDto("user2", "user2@example.com", "Secret123");
        var id = await service.RegisterUserAsync(dto);

        var userDto = await service.GetUserAsync(id);

        Assert.Equal("user2", userDto.Username);
        Assert.Equal("user2@example.com", userDto.Email);
    }

    [Fact]
    public async Task GetUserAsync_Should_Throw_When_NotFound()
    {
        var context = GetDbContext();
        var service = new UserService(context);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.GetUserAsync(Guid.NewGuid()));
    }
}