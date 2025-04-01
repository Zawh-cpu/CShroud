using Avalonia;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Core.Services;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Platforms.Linux.Services;
using CShroudApp.Infrastructure.Platforms.Unsupported.Services;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Presentation.Ui;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{

    static int Main()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ApplicationConfig>(provider => new ApplicationConfig()
        {
            WorkingFolder = Environment.CurrentDirectory,
            Settings = new SettingsConfig()
            {
                VpnMode = VpnMode.Proxy
            }
        });

        serviceCollection.AddSingleton<VpnCoreConfig>(provider => new VpnCoreConfig()
        {
            Path = "Presentation\\VPNCore\\sing-box.exe",
            ConfigPath = "Presentation\\VPNCore\\config.json",
            Arguments = $"run -c Presentation\\VPNCore\\config.json",
            Debug = true
        });


        serviceCollection.AddSingleton<ICore, Core>();
        serviceCollection.AddSingleton<IProcessManager, ProcessManager>();
        serviceCollection.AddSingleton<IVpnCore, VpnCore>();

        switch (PlatformService.Platform)
        {
            case Platform.Windows:
                serviceCollection.AddSingleton<IProxyManager, WindowsProxyManager>();
                break;
            case Platform.Linux:
                serviceCollection.AddSingleton<IProxyManager, LinuxProxyManager>();
                break;
            default:
                serviceCollection.AddSingleton<IProxyManager, UnsupportedProxyManager>();
                break;
        }
        
        serviceCollection.AddSingleton<IServerRepository, ServerRepository>();
        serviceCollection.AddSingleton<IVpnService, VpnService>();

        serviceCollection.AddSingleton<UiLoader>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var core = serviceProvider.GetRequiredService<ICore>()!;
        core.Initialize();
        core.Start();

        return 0;
    }

    [STAThread]
    static void Run(string[] args) => UiLoader.Run(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    static AppBuilder BuildAvaloniaApp() => UiLoader.BuildAvaloniaApp();    
}