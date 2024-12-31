namespace CShroud.Infrastructure.Data.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(96)]
    public string? Nickname { get; set; }
    
    [MaxLength(96)]
    public string? Login { get; set; }
    
    [MaxLength(128)]
    public String? Password { get; set; }
    
    
    public UInt64? TelegramId { get; set; }
    
    [Required] public int RoleId { get; set; } = 1;
    
    // [ForeignKey("RoleId")]
    // public SQL.Models.Role? Role { get; set; }
    
    [Required] public int RateId { get; set; } = 1;
    
    // [ForeignKey("RateId")]
    // public SQL.Models.Rate? Rate { get; set; }
    
    // public List<SQL.Models.Key> Keys { get; set; } = new List<SQL.Models.Key>();
    
    public DateTime? PayedUntil { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow.AddHours(3);
    public DateTime? TelegramJoinedAt { get; set; } = DateTime.UtcNow.AddHours(3);
    
    public bool IsActive { get; set; } = true;

}