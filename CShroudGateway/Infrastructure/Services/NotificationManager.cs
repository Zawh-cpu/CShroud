using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Services;

public class NotificationManager : INotificationManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationManager()
    {
        _httpClientFactory = new();
    }
    
    private void SendInTelegram(Notification notification)
    {
        
    }
    
    public Task ExecuteAndForget(List<Notification> notifications)
    {
        throw new NotImplementedException();
    }
}