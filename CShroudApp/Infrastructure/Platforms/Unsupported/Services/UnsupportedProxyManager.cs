using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Unsupported.Services;

public class UnsupportedProxyManager : IProxyManager
{
    public void Enable(string proxyAddress) {}

    public void Disable() {}
}