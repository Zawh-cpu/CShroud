using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class Notifications
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(nameof(Recipient))]
    public Guid? RecipientId { get; set; }
    public User? Recipient { get; set; }

    public Guid SenderId { get; set; } = Guid.AllBitsSet;
    
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [MaxLength(500)]
    public required string Content { get; set; }
    
    
}