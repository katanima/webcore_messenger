using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webcore_backend.Features.Users.Entities;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildMemberEntity
{
    public Guid UserId { get; set; }
    public Guid GuildId { get; set; }
    
    [Required]
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
    [Required]
    [ForeignKey(nameof(GuildId))]
    public GuildEntity Guild { get; set; }

    public List<GuildRoleEntity> Roles { get; set; } = [];
}