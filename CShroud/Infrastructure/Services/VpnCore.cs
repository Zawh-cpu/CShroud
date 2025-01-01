using System.Diagnostics;
using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class VpnCore: IVpnCore
{
    private readonly BaseProcess _process;
    private IProcessManager _processManager;
    private VpnCoreConfig _vpnConfig;
    public bool IsRunning => _process.IsRunning;
    
    public VpnCore(VpnCoreConfig vpnConfig, IProcessManager processManager)
    {
        _vpnConfig = vpnConfig;
        _processManager = processManager;

        var processStartInfo = new ProcessStartInfo
        {
            FileName = _vpnConfig.Path,
            Arguments = "",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        _process = new BaseProcess(processStartInfo);
        _processManager.Register(_process);
        
        // _process.Start();
    }

    private void MakeRequest()
    {
        try
        {

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void SystemInfo()
    {
        
    }


}