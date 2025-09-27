using webcore_backend.Features.Guilds.Dtos;
using webcore_backend.Features.Guilds.Entities;
using webcore_backend.Features.Users.Services;
using webcore_backend.Models;

namespace webcore_backend.Features.Guilds.Services;

public class GuildService(AppDbContext _dbContext, HttpContext _httpContext, IUserService _userService) : IGuildService
{
    public Task<Guid> CreateGuildAsync(CreateGuildRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task EditGuildAsync(EditGuildRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task JoinGuildByInviteAsync(Guid inviteId)
    {
        throw new NotImplementedException();
    }

    public Task LeaveGuildAsync(Guid guildId)
    {
        throw new NotImplementedException();
    }
}