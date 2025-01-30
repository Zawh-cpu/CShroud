using System.Runtime.InteropServices;
using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Infrastructure.Platforms.Windows.Services;

namespace CShroudApp.Infrastructure.Services;

public class ProxyService : IProxyService
{
    private IProxyManager proxyManager;
    
    public ProxyService(IPlatformService platformService)
    {
        switch (platformService.Platform)
        {
            case OSPlatform.Windows:
                proxyManager = new WindowsProxyManager();
                break;
        }
    }
    
    public void Enable(string proxyAddress)
    {
        
    }

    public void Disable()
    {
        
    }
}