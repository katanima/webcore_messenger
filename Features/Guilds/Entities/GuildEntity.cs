using System.ComponentModel.DataAnnotations;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public string WebsiteUrl { get; set; }

    public List<GuildMemberEntity> Members { get; set; } = [];

    public List<GuildInviteEntity> Invites { get; set; } = [];

    public List<GuildRoleEntity> Roles { get; set; } = [];
}