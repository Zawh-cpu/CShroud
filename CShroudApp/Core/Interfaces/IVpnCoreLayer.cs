using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCoreLayer
{
    public bool IsProtocolSupported(VpnProtocol protocol);
    
    public void AddInbound(IVpnBound bound);
    public void AddOutbound(IVpnBound bound);
    public void RemoveInbound(string tag, bool startsWithMode = false);
    public void RemoveOutbound(string tag, bool startsWithMode = false);

    public void SaveConfiguration();
}