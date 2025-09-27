using System.ComponentModel.DataAnnotations;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildInviteEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid GuildId { get; set; }
    
    [Required]
    public Guid ChannelId { get; set; }
    
    [Required]
    public DateTime expirationDate { get; set; }
    
    public int InviteCount { get; set; }
    
    public int MaxUseAmount { get; set; }
}