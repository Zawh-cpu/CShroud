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

    public bool IsRunning => _vpnCoreLayer.IsRunning;
    
    public bool IsSupportProtocol(VpnProtocol protocol) => _vpnCoreLayer.IsProtocolSupported(protocol);
    public List<VpnProtocol> SupportedProtocols => _vpnCoreLayer.SupportedProtocols;
    public void SaveConfiguration() => _vpnCoreLayer.SaveConfiguration();

    public VpnCore(IVpnCoreLayer vpnCoreLayer)
    {
        _vpnCoreLayer = vpnCoreLayer;
        _vpnCoreLayer.ProcessStarted += OnProcessStarted;
        _vpnCoreLayer.ProcessExited += OnProcessStopped;
    }

    public async Task EnableAsync()
    {
        if (!IsRunning)
        {
            _vpnCoreLayer.StartProcess();
        }
    }

    public async Task DisableAsync()
    {
        if (IsRunning)
        {
            await _vpnCoreLayer.KillProcessAsync();
        }
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        _vpnCoreLayer.FixDnsIssues(transparentHosts);
    }
    
    public void ChangeMainInbound(VpnMode mode)
    {
;
        _vpnCoreLayer.RemoveInbound("main-net-", true);
        
        _vpnCoreLayer.AddInbound(new Socks()
        {
            Tag = "main-net-socks",
            Host = "127.0.0.1",
            Port = 10818,
            Sniff = true,
            SniffOverrideDestination = true
        });
        
        _vpnCoreLayer.AddInbound(new Http()
        {
            Tag = "main-net-http",
            Host = "127.0.0.1",
            Port = 10819,
            Sniff = true,
            SniffOverrideDestination = true
        });
    }
    
    public void ChangeMainOutbound(IVpnBound bound)
    {
        _vpnCoreLayer.RemoveOutbound("main-net-", true);
        bound.Tag = "main-net-outbound";
        _vpnCoreLayer.AddOutbound(bound, 0);
    }

    private void OnProcessStarted(object? sender, EventArgs e)
    {
        VpnEnabled?.Invoke(this, e);
    }

    private void OnProcessStopped(object? sender, EventArgs e)
    {
        VpnDisabled?.Invoke(this, e);
    }

    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
}