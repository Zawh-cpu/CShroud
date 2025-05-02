using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface INotificationManager
{
    Task ExecuteAndForget(List<Notification> notifications);
}