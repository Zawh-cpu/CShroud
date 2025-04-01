namespace CShroudApp.Core.Interfaces;

public interface IProxyManager
{
    void Enable(string proxyAddress);
    void Disable();
}