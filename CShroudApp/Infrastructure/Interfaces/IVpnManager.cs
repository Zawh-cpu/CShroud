namespace CShroudApp.Infrastructure.Interfaces;

public interface IVpnManager
{
    void UpdateConfig();
    void Start();
    void Stop();
}