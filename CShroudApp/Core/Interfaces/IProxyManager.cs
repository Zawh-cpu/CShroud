namespace CShroudApp.Core.Interfaces;

public interface IProxyManager
{
    Task Enable(string proxyAddress, List<string> excludedHosts);
    Task Disable(string? oldAddress, List<string>? excludedHosts);
}