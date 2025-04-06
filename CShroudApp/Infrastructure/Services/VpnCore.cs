using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnCore : IVpnCore
{
    private readonly IVpnCoreLayer _vpnCoreLayer;

    public bool IsSupportProtocol(VpnProtocol protocol) => _vpnCoreLayer.IsProtocolSupported(protocol);
    public void SaveConfiguration() => _vpnCoreLayer.SaveConfiguration();

    public VpnCore(IVpnCoreLayer vpnCoreLayer)
    {
        _vpnCoreLayer = vpnCoreLayer;
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
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

    public bool IsRunning()
    {
        throw new NotImplementedException();
    }

    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
}