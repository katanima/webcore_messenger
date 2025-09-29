using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webcore_backend.Features.Guilds.Entities;

public class GuildInviteEntity
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    
    [Required]
    [ForeignKey(nameof(GuildId))]
    public GuildEntity Guild { get; set; }
    
    public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
    
    public int RemainingUses { get; set; } = int.MaxValue;
    
    public int InviteCount { get; set; }
}