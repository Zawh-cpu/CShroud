namespace CShroudApp.Infrastructure.Interfaces;

public interface IProxyService
{
    void Enable(string proxyAddress);
    void Disable();
}