namespace CShroudApp.Infrastructure.Interfaces;

public interface IVpnService
{
    void UpdateConfig();
    void Start();
    void Stop();
}