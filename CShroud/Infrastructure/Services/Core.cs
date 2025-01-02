using CShroud.Core.Domain.Handlers;
using CShroud.Infrastructure.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class Core : ICore
{
    private readonly IServiceProvider _serviceProvider;
    private IBaseRepository _baseRepository;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IServiceProvider serviceProvider, IBaseRepository repo, IVpnRepository vpnRepo)
    {
        _serviceProvider = serviceProvider;
        _baseRepository = repo;

        vpnRepo.AddKey(0, "311314124124", "vless").GetAwaiter().GetResult();
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
        var processManager = _serviceProvider.GetRequiredService<IProcessManager>();
        // _baseRepository = _serviceProvider.GetRequiredService<IBaseRepository>();
        Console.WriteLine(_baseRepository.Ping());
    }
}