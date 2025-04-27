using System.ComponentModel.DataAnnotations.Schema;
using CShroudGateway.Core.Entities;

namespace CShroudGateway.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;

public class Key
{
    [System.ComponentModel.DataAnnotations.Key]
    public Guid Id { get; set; }
    
    [MaxLength(64)]
    public string? Name { get; set; }
    
    [ForeignKey(nameof(Server))]
    public required Guid ServerId { get; set; }
    public Server? Server { get; set; }
    
    public required VpnProtocol Protocol { get; set; }
    
    
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; set; }
    public User? User { get; set; }
    
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    public bool IsRevoked { get; set; } = false;
}