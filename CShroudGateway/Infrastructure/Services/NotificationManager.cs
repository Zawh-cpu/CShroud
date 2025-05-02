using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    private void SendNotifyToDestination(Notification notification)
    {
        
    }
    
    public Task ExecuteAndForget(List<Notification> notifications)
    {
        throw new NotImplementedException();
    }
}