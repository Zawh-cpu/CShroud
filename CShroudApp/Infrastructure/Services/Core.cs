using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Ui;

namespace CShroudApp.Infrastructure.Services;

public class Core : ICore
{
    private readonly IVpnCore _vpnCore;
    private readonly IServerRepository _serverRepository;
    private readonly IVpnService _vpnService;
    private readonly UiLoader _uiLoader;

    public Core(IVpnCore vpnCore, IServerRepository serverRepository, IVpnService vpnService, UiLoader gui)
    {
        _vpnCore = vpnCore;
        _serverRepository = serverRepository;
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
        _serverRepository.Login();
        _vpnService.Start(VpnMode.Proxy);
        // UiLoader.Run([]);

        while (true)
        {
            Console.WriteLine(_vpnCore.IsRunning);
            Task.Delay(10000).Wait();
        };
    }
}