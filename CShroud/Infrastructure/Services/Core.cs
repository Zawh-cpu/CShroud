using Microsoft.Extensions.DependencyInjection;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class Core : ICore
{
    private readonly IServiceProvider _serviceProvider;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        
        while (true)
        {
            
        }
    }
}