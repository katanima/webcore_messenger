using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webcore_backend.Features.Guilds.Invites.Entities;
using webcore_backend.Features.Guilds.Members.Entities;
using webcore_backend.Features.Guilds.Roles.Entities;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Features.Guilds.Core.Entities;

public class GuildEntity
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    [Required]
    [ForeignKey(nameof(OwnerId))]
    public UserEntity Owner { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public string WebsiteUrl { get; set; }

    public List<GuildMemberEntity> Members { get; set; } = [];

    public List<GuildInviteEntity> Invites { get; set; } = [];

    public List<GuildRoleEntity> Roles { get; set; } = [];
}