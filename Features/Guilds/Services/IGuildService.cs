using webcore_backend.Features.Guilds.Dtos;

namespace webcore_backend.Features.Guilds;

public interface IGuildService
{
    public Task<Guid> CreateGuildAsync(CreateGuildRequestDto dto);
    public Task EditGuildAsync(EditGuildRequestDto dto);
    public Task JoinGuildByInviteAsync(Guid inviteId);
    public Task LeaveGuildAsync(Guid guildId);
}