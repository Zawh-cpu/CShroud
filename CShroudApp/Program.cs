using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnLayers;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<IApiRepository, ApiRepository>();
services.AddSingleton<IVpnCore, VpnCore>();
services.AddSingleton<IVpnService, VpnService>();


// Optional
services.AddSingleton<IVpnCoreLayer, SingBoxLayer>();



var service = services.BuildServiceProvider();

var vpnService = service.GetRequiredService<IVpnService>();
vpnService.Enable(VpnMode.Proxy).GetAwaiter().GetResult();