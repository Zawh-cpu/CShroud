using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Presentation.Ui;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnService _vpnService;
    private readonly UiLoader _uiLoader;

    public Core(IVpnService vpnService, UiLoader gui)
    {
        _vpnService = vpnService;
        _uiLoader = gui;
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
        _uiLoader.Run([]);
    }
}