using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<ApplicationConfig>(provider => new ApplicationConfig()
{
    WorkingFolder = Environment.CurrentDirectory
});

serviceCollection.AddSingleton<ICore, Core>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var core = serviceProvider.GetService<Core>()!;
core.Initialize();
core.Start();
