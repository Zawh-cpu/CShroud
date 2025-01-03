using CShroud.Core.Domain.Handlers;
using CShroud.Infrastructure.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Tasks;

namespace CShroud.Infrastructure.Services;

public class Core : ICore
{
    private readonly IServiceProvider _serviceProvider;
    private IBaseRepository _baseRepository;
    private IVpnCore _vpnCore;
    private IPlanner _planner;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IServiceProvider serviceProvider, IBaseRepository repo, IVpnRepository vpnRepo, IVpnCore vpnCore, IPlanner planner)
    {
        _serviceProvider = serviceProvider;
        _baseRepository = repo;
        _vpnCore = vpnCore;
        _planner = planner;
        
        // vpnRepo.AddKey(0, "311314124124", "vless").GetAwaiter().GetResult();
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
        var task = new TestTask(DateTime.UtcNow.AddSeconds(5), _vpnCore);
        _planner.AddTask(task);
        var processManager = _serviceProvider.GetRequiredService<IProcessManager>();
        // _baseRepository = _serviceProvider.GetRequiredService<IBaseRepository>();
        Console.WriteLine(_baseRepository.Ping());
    }
}