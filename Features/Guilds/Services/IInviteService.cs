using webcore_backend.Features.Guilds.Dtos;

namespace webcore_backend.Features.Invites.Services;

public interface IInviteService
{
    public Task CreateInvitationAsync(CreateInvitationRequestDto dto);
    public Task CancelInvitationAsync(Guid inviteId);
}