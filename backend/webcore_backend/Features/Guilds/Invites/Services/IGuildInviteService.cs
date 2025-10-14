using webcore_backend.Features.Guilds.Invites.Dtos;
using webcore_backend.Features.Guilds.Invites.Entities;

namespace webcore_backend.Features.Guilds.Invites.Services;

public interface IGuildInviteService
{
    public Task CreateInvitationAsync(Guid currentUserId, CreateInvitationRequestDto dto);
    public Task CancelInvitationAsync(Guid inviteId);
    public Task<GuildInviteEntity> RequireValidInviteAsync(Guid inviteId);
    public Task UseInviteAsync(GuildInviteEntity invite);
}