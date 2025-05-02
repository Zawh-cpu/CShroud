using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CShroudGateway.Infrastructure.Data.Entities;

public enum MailType
{
    Message,
    RateExpiration,
    RateExpired
}

public class Mail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(nameof(Recipient))]
    public Guid? RecipientId { get; set; }
    public User? Recipient { get; set; }

    public Guid SenderId { get; set; }

    public MailType Type { get; set; } = MailType.Message;
    
    [MaxLength(100)]
    public required string? Title { get; set; }
    
    [MaxLength(500)]
    public required string? Content { get; set; }
    
    public Dictionary<string, object> ExtraData { get; set; } = new();
    
    
}