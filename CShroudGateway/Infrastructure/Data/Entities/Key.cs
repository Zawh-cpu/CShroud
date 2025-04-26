using System.ComponentModel.DataAnnotations.Schema;

namespace CShroudGateway.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;

public class Key
{
    [System.ComponentModel.DataAnnotations.Key]
    public uint Id { get; set; }
    
    public required string Uuid { get; set; }
    
    [MaxLength(64)]
    public string? Name { get; set; }
    
    public required string LocationId { get; set; } = "frankfurt";
    
    public required string ProtocolId { get; set; }
    public Protocol? Protocol { get; set; }
    
    
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; set; }
    public User? User { get; set; }
    
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    public bool IsRevoked { get; set; } = false;
}