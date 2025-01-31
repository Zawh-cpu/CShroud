using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Presentation.Ui;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnService _vpnService;
    
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
        Console.WriteLine("CORE STARTED");
        UiLoader.Run([]);
    }
}