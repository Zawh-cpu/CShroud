using System.Diagnostics;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Infrastructure.Services;

public class ProcessFactory : IProcessFactory
{
    private readonly IProcessManager _processManager;
    private readonly IElevationManager _elevationManager;
    
    public ProcessFactory(IElevationManager elevationManager, IProcessManager processManager)
    {
        _elevationManager = elevationManager;
        _processManager = processManager;
    }
    
    public void Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None)
    {
        var process = new BaseProcess(processStartInfo, debug, _elevationManager, _processManager);
        return process
    }
}