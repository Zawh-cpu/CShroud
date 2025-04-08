using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpnCore = CShroudApp.Infrastructure.Services.VpnCore;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.Configure<PathConfig>(config.GetSection("Path"));
services.Configure<SettingsConfig>(config.GetSection("Settings"));

services.AddSingleton<IProcessManager, ProcessManager>();
services.AddSingleton<IApiRepository, ApiRepository>();
services.AddSingleton<IVpnCore, VpnCore>();
services.AddSingleton<IVpnService, VpnService>();


// Optional
services.AddSingleton<IVpnCoreLayer, SingBoxLayer>();



var service = services.BuildServiceProvider();

var vpnService = service.GetRequiredService<IVpnService>();
vpnService.Enable(VpnMode.Proxy).GetAwaiter().GetResult();