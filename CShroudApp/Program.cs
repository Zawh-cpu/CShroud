using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers.SingBox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpnCore = CShroudApp.Infrastructure.Services.VpnCore;

void Aboba(object? ev, EventArgs args)
{
    Console.WriteLine("CShroud App -> VPN enabled");
}

void AbobaOff(object? ev, EventArgs args)
{
    Console.WriteLine("CShroud App -> VPN disabled/crashed");
}

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.Configure<SettingsConfig>(config.GetSection("Settings"));

services.AddSingleton<IProcessManager, ProcessManager>();
services.AddSingleton<IApiRepository, ApiRepository>();
services.AddSingleton<IVpnCore, VpnCore>();

if (OperatingSystem.IsWindows())
{
    services.AddSingleton<IProxyManager, WindowsProxyManager>();
} else if (OperatingSystem.IsLinux())
{
    
}
else
{
    Console.WriteLine("This platform currently is not supported. Sorry :(");
    Environment.Exit(1);
}

services.AddSingleton<IVpnService, VpnService>();


// Optional
services.AddSingleton<IVpnCoreLayer, SingBoxLayer>();



var service = services.BuildServiceProvider();

var vpnService = service.GetRequiredService<IVpnService>();
vpnService.VpnEnabled += Aboba;
vpnService.VpnDisabled += AbobaOff;
vpnService.EnableAsync(VpnMode.Tun).GetAwaiter().GetResult();
while (true)
{
    
}
