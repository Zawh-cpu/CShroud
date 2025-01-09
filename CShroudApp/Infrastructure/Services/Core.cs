using Microsoft.Extensions.DependencyInjection;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnManager _vpnManager;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IVpnManager vpnManager)
    {
        _vpnManager = vpnManager;
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
        _vpnManager.Start();
        
        while (true)
        {
            
        }
    }
}