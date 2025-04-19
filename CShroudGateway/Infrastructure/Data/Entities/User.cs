namespace CShroudGateway.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public uint Id { get; set; }
    
    [MaxLength(96)]
    public string? Nickname { get; set; }
    
    [MaxLength(96)]
    public string? Login { get; set; }
    
    [MaxLength(128)]
    public String? Password { get; set; }
    
    
    public ulong? TelegramId { get; set; }
    public DateTime? PayedUntil { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? TelegramJoinedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;

}