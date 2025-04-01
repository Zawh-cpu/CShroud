using System.Runtime.Versioning;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Linux.Services;


[SupportedOSPlatform("linux")]
public class LinuxProxyManager : IProxyManager
{
    public void Enable(string proxyAddress) {}

    public void Disable() {}
}