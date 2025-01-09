namespace CShroudApp.Infrastructure.Interfaces;

public interface IProxyManager
{
    void Enable(string proxyAddress);
    void Disable();
}