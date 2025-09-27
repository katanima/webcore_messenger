using System.ComponentModel.DataAnnotations;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildMemberEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid GuildId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public List<GuildRoleEntity> Roles { get; set; }
}