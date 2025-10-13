using webcore_backend.Features.Guilds.Members.Entities;
using webcore_backend.Features.Guilds.Roles.Models;

namespace webcore_backend.Features.Guilds.Members.Services;

public interface IGuildMemberService
{
    public bool HasAnyPermission(GuildMemberEntity member, params RolePermissions[] permissions);
    public Task AddMemberToGuildAsync(Guid currentUserId, Guid guildId);
    public Task RemoveMemberFromGuildAsync(Guid currentUserId, Guid guildId);
}