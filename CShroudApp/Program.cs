using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;
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
/*serviceCollection.AddSingleton<IPlatformService>(platformService);

switch (platformService.PlatformString)
{
    case "windows":
}*/

serviceCollection.AddSingleton<IVpnService, VpnService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var core = serviceProvider.GetRequiredService<ICore>()!;
core.Initialize();
core.Start();
