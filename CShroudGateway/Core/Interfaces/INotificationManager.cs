using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public record Notification
{
    public enum NotificationType
    {
        RateExpired,
        RateExpiration
    }
    
    public required NotificationType Type { get; set; }
    public required User User { get; set; }
    public Dictionary<string, object>? ExtraData { get; set; }
}


public interface INotificationManager
{
    Task ExecuteAndForget(List<Notification> notifications);
}