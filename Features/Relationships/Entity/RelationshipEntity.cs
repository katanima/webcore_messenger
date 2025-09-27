using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webcore_backend.Features.Friends.Models;
using webcore_backend.Features.Users.Entities;

namespace webcore_backend.Features.Friends.Entity;

public class RelationshipEntity
{
    public Guid UserAId { get; set; }
    public Guid UserBId { get; set; }
    
    [Required]
    [ForeignKey(nameof(UserAId))]
    public UserEntity UserA { get; set; }
    [Required]
    [ForeignKey(nameof(UserBId))]
    public UserEntity UserB { get; set; }
    
    [Required] 
    public RelationshipStatus Status { get; private set; }

    [Required] 
    public DateTime ChangedStatus { get; private set; }
    
    public void SetStatus(RelationshipStatus status)
    {
        if (Status == status) return;
        Status = status;
        ChangedStatus = DateTime.UtcNow;
    }
}