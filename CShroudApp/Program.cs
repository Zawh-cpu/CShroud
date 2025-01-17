using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateApplicationBuilder();

host.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

host.Services.AddSingleton<ApplicationConfig>(provider => new ApplicationConfig()
{
    WorkingFolder = Environment.CurrentDirectory
});


host.Services.AddSingleton<VpnCoreConfig>(provider => new VpnCoreConfig());

host.Services.AddSingleton<IProcessManager, ProcessManager>();
host.Services.AddSingleton<IVpnCore, VpnCore>();
host.Services.AddSingleton<IProxyManager, ProxyManager>();
host.Services.AddSingleton<IVpnService, VpnService>();
host.Services.AddSingleton<ICore, Core>();

var app = host.Build();

var core = app.Services.GetRequiredService<ICore>();
core.Initialize();
core.Start();
