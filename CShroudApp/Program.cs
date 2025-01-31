using CShroudApp.Core.Domain.Entities;
using CShroudApp.Core.Services;
using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Infrastructure.Platforms.Linux.Services;
using CShroudApp.Infrastructure.Platforms.Unsupported.Services;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<ApplicationConfig>(provider => new ApplicationConfig()
{
    WorkingFolder = Environment.CurrentDirectory
});

serviceCollection.AddSingleton<VpnCoreConfig>(provider => new VpnCoreConfig()
{
    Path = "",
    Link = "",
    Arguments = "",
    Debug = false
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

serviceCollection.AddSingleton<IVpnService, VpnService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var core = serviceProvider.GetRequiredService<ICore>()!;
core.Initialize();
core.Start();
