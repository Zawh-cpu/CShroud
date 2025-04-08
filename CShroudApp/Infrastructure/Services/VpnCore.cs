using System.Diagnostics;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class VpnCore : IVpnCore
{
    private readonly IVpnCoreLayer _vpnCoreLayer;
    private readonly IProcessManager _processManager;
    private readonly BaseProcess _process;
    private readonly PathConfig _pathConfig;
    private readonly SettingsConfig  _settingsConfig;
    
    public bool IsRunning { get; } = true;
    
    public bool IsSupportProtocol(VpnProtocol protocol) => _vpnCoreLayer.IsProtocolSupported(protocol);
    public void SaveConfiguration() => _vpnCoreLayer.SaveConfiguration();

    public VpnCore(IVpnCoreLayer vpnCoreLayer, IProcessManager processManager, IOptions<PathConfig> pathConfig, IOptions<SettingsConfig> settingsConfig)
    {
        _vpnCoreLayer = vpnCoreLayer;
        _processManager = processManager;
        _pathConfig = pathConfig.Value;
        _settingsConfig = settingsConfig.Value;
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = _vpnConfig.Path,
            Arguments = _vpnConfig.Arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        //_process = new BaseProcess(processStartInfo, debug: _vpnConfig.Debug);
        
        //_process.ProcessStarted += OnProcessStarted;
        //_process.ProcessExited += OnProcessStopped;
        
        //_processManager.Register(_process);
    }

    public async Task Enable()
    {
    }

    public async Task Disable()
    {
    }

    public void ChangeMainInbound(VpnMode mode)
    {
;
        _vpnCoreLayer.RemoveInbound("main-net-", true);
        
        _vpnCoreLayer.AddInbound(new Socks()
        {
            Tag = "main-net-socks",
            Host = "127.0.0.1",
            Port = 10808,
            Sniff = true,
            SniffOverrideDestination = true
        });
        
        _vpnCoreLayer.AddInbound(new Http()
        {
            Tag = "main-net-http",
            Host = "127.0.0.1",
            Port = 10809,
            Sniff = true,
            SniffOverrideDestination = true
        });
    }

    public void ChangeMainOutbound(IVpnBound bound)
    {
        _vpnCoreLayer.RemoveOutbound("main-net-", true);
        bound.Tag = "main-net-outbound";
        _vpnCoreLayer.AddOutbound(bound);
    }

    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
}