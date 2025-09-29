using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Guilds.Entities;
using webcore_backend.Features.Guilds.Models;
using webcore_backend.Models;

namespace webcore_backend.Features.Guilds.Services;

public class GuildMemberService(AppDbContext _dbContext) : IGuildMemberService
{
    public bool HasAnyPermission(GuildMemberEntity member, params RolePermissions[] permissions)
        => !member.Roles.Any(r => permissions.Any(p =>
            r.Permissions.HasFlag(p)));
    
    public async Task AddMemberToGuildAsync(Guid currentUserId, Guid guildId)
    {
        var exists = await _dbContext.GuildMember
            .AnyAsync(m => m.UserId == currentUserId && m.GuildId == guildId);
        if (exists)
            throw new InvalidOperationException($"User with id {currentUserId} is already in the guild with id {guildId}");

        await _dbContext.GuildMember.AddAsync(new GuildMemberEntity
        {
            UserId = currentUserId,
            GuildId = guildId
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveMemberFromGuildAsync(Guid currentUserId, Guid guildId)
    {
        var member = await _dbContext.GuildMember
            .FirstOrDefaultAsync(m => m.UserId == currentUserId && m.GuildId == guildId)
            ?? throw new InvalidOperationException($"User with id {currentUserId} is not in the guild with id {guildId}");
        
        _dbContext.GuildMember.Remove(member);
        await _dbContext.SaveChangesAsync();
    }
}