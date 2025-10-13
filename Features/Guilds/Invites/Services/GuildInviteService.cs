using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Guilds.Invites.Dtos;
using webcore_backend.Features.Guilds.Invites.Entities;
using webcore_backend.Features.Guilds.Members.Services;
using webcore_backend.Features.Guilds.Roles.Models;
using webcore_backend.Models;
using webcore_backend.Shared.Exceptions;

namespace webcore_backend.Features.Guilds.Invites.Services;

public class GuildInviteService(AppDbContext _dbContext, IGuildMemberService _memberService) : IGuildInviteService
{
    public async Task CreateInvitationAsync(Guid currentUserId, CreateInvitationRequestDto request)
    {
        var currentGuildMember = await _dbContext.GuildMember.FirstOrDefaultAsync(m => m.UserId == currentUserId && m.GuildId == request.GuildId)
                                 ?? throw new KeyNotFoundException("Member not found");

        var hasPermission = _memberService.HasAnyPermission(
            currentGuildMember,
            RolePermissions.Administrator,
            RolePermissions.ManageInvites);
        if (!hasPermission)
            throw new MissingPermissionException($"Member with id {currentGuildMember.UserId} has no permissions");
        
        var invite = new GuildInviteEntity
        {
            GuildId = request.GuildId,
            ExpirationDate = request.ExpirationDate ?? DateTime.MaxValue,
            RemainingUses = request.Usage ?? int.MaxValue,
            InviteCount = 0
        };

        _dbContext.GuildInvite.Add(invite);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CancelInvitationAsync(Guid inviteId)  
    {
        var invite = await _dbContext.GuildInvite.FindAsync(inviteId);
        if (invite == null)
            throw new KeyNotFoundException("Invite not found");

        _dbContext.GuildInvite.Remove(invite);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<GuildInviteEntity> RequireValidInviteAsync(Guid inviteId)
    {
        var invite = await _dbContext.GuildInvite
                         .Include(i => i.Guild)
                         .FirstOrDefaultAsync(i => i.Id == inviteId)
                     ?? throw new KeyNotFoundException("Invite not found");

        if (invite.ExpirationDate < DateTime.UtcNow)
            throw new InvalidOperationException("Invite expired");

        if (invite.RemainingUses <= 0)
            throw new InvalidOperationException("Invite exhausted");

        return invite;
    }

    public Task UseInviteAsync(GuildInviteEntity invite)
    {
        invite.RemainingUses--;
        invite.InviteCount++;
        return _dbContext.SaveChangesAsync();
    }
}
