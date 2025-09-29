using webcore_backend.Features.Guilds.Dtos;
using webcore_backend.Features.Guilds.Entities;

namespace webcore_backend.Features.Invites.Services;

public interface IGuildInviteService
{
    public Task CreateInvitationAsync(Guid currentUserId, CreateInvitationRequestDto dto);
    public Task CancelInvitationAsync(Guid inviteId);
    public Task<GuildInviteEntity> RequireValidInviteAsync(Guid inviteId);
    public Task UseInviteAsync(GuildInviteEntity invite);
}