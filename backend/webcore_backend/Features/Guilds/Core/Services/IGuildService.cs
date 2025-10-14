using webcore_backend.Features.Guilds.Core.Dtos;

namespace webcore_backend.Features.Guilds.Core.Services;

public interface IGuildService
{
    public Task<Guid> CreateGuildAsync(Guid currentUserId, CreateGuildRequestDto request);
    public Task EditGuildAsync(Guid currentUserId, EditGuildRequestDto request);
    public Task JoinGuildAsync(Guid currentUserId, Guid inviteId);
    public Task LeaveGuildAsync(Guid currentUserId, Guid guildId);
}