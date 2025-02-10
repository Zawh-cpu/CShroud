using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Presentation.Ui;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnService _vpnService;
    private readonly IVpnCore _vpnCore;
    private readonly UiLoader _uiLoader;

    public Core(IVpnService vpnService, IVpnCore vpnCore, UiLoader gui)
    {
        _vpnService = vpnService;
        _vpnCore = vpnCore;
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
        _vpnService.Start();
        // UiLoader.Run([]);

        while (true)
        {
            Console.WriteLine(_vpnCore.IsRunning);
            Task.Delay(10000).Wait();
        };
    }
}