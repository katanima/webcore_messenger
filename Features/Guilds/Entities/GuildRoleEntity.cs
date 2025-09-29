using System.ComponentModel.DataAnnotations;
using webcore_backend.Features.Guilds.Models;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildRoleEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid GuildId { get; set; }
    
    [Required]
    public RolePermissions Permissions { get; set; }
}