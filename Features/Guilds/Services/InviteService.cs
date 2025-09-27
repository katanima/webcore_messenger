using webcore_backend.Features.Guilds.Dtos;
using webcore_backend.Features.Invites.Services;

namespace webcore_backend.Features.Guilds.Services;

public class InviteService : IInviteService
{
    public Task CreateInvitationAsync(CreateInvitationRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task CancelInvitationAsync(Guid inviteId)
    {
        throw new NotImplementedException();
    }
}