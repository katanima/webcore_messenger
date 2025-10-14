using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Guilds.Core.Dtos;
using webcore_backend.Features.Guilds.Core.Entities;
using webcore_backend.Features.Guilds.Invites.Services;
using webcore_backend.Features.Guilds.Members.Services;
using webcore_backend.Features.Guilds.Roles.Models;
using webcore_backend.Models;
using webcore_backend.Shared.Exceptions;

namespace webcore_backend.Features.Guilds.Core.Services;

public class GuildService(AppDbContext _dbContext, IGuildInviteService _inviteService, IGuildMemberService _memberService) : IGuildService
{
    private async Task<GuildEntity> RequireGuildAsync(Guid guildId) 
        => await _dbContext.Guild.FindAsync(guildId) 
           ?? throw new KeyNotFoundException($"Guild with id {guildId} not found");

    private async Task UserExistsOrThrowAsync(Guid currentUserId)
    {
        if (!await _dbContext.User.AnyAsync(u => u.Id == currentUserId))
            throw new UnauthorizedAccessException($"User with id {currentUserId} not found");
    }
    
    public async Task<Guid> CreateGuildAsync(Guid currentUserId, CreateGuildRequestDto request)
    {
        await UserExistsOrThrowAsync(currentUserId);
        
        var guild = new GuildEntity
        {
            Name = request.Name,
            Description = request.Description
        };
        
        await _dbContext.Guild.AddAsync(guild);
        await _dbContext.SaveChangesAsync();
        
        await _memberService.AddMemberToGuildAsync(currentUserId, guild.Id);

        return guild.Id;
    }
    
    public async Task EditGuildAsync(Guid currentUserId, EditGuildRequestDto request)
    {
        var currentGuildMember = await _dbContext.GuildMember
                                     .Include(m => m.Roles)
                                     .FirstOrDefaultAsync(m => m.UserId == currentUserId && m.GuildId == request.GuildId) 
                                 ?? throw new UnauthorizedAccessException($"User with id {request.GuildId} is not member of the guild or guild with id {request.GuildId} doesn't exist");

        var hasPermissions = _memberService.HasAnyPermission(
            currentGuildMember,
            RolePermissions.Administrator,
            RolePermissions.EditServer);
        if (!hasPermissions)
            throw new MissingPermissionException($"Member with id {currentGuildMember.UserId} has no permissions");
        
        var guild = await RequireGuildAsync(request.GuildId);
        
        guild.Name = request.Name ?? guild.Name;
        guild.Description = request.Description ?? guild.Description;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task JoinGuildAsync(Guid currentUserId, Guid inviteId)
    {
        var invite = await _inviteService.RequireValidInviteAsync(inviteId);

        await _memberService.AddMemberToGuildAsync(currentUserId, invite.Guild.Id);

        await _inviteService.UseInviteAsync(invite);
    }
    
    public async Task LeaveGuildAsync(Guid currentUserId, Guid guildId)
    {
        await _memberService.RemoveMemberFromGuildAsync(currentUserId, guildId);
    }
}