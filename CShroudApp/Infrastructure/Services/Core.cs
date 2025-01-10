using Microsoft.Extensions.DependencyInjection;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnService _vpnService;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IVpnService vpnService)
    {
        _vpnService = vpnService;
    }

    public static string BuildPath(params string[] paths)
    {
        return Path.Combine(Environment.CurrentDirectory, Path.Combine(paths));
    }
    
    public void Initialize()
    {
    }

    public void Shutdown()
    {
    }

    public void Start()
    {
        _vpnService.Start();
        
        while (true)
        {
            
        }
    }
}