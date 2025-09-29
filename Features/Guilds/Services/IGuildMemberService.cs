using webcore_backend.Features.Guilds.Entities;
using webcore_backend.Features.Guilds.Models;

namespace webcore_backend.Features.Guilds.Services;

public interface IGuildMemberService
{
    public bool HasAnyPermission(GuildMemberEntity member, params RolePermissions[] permissions);
    public Task AddMemberToGuildAsync(Guid currentUserId, Guid guildId);
    public Task RemoveMemberFromGuildAsync(Guid currentUserId, Guid guildId);
}