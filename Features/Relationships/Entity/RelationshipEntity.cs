using System.ComponentModel.DataAnnotations;
using webcore_backend.Features.Friends.Models;

namespace webcore_backend.Features.Friends.Entity;

public class RelationshipEntity
{
    public Guid Id { get; set; }

    [Required] 
    public Guid SenderId { get; set; }

    [Required] 
    public Guid ReceiverId { get; set; }
    
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