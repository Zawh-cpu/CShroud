using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    Task Enable();
    Task Disable();
    
    void ChangeMainInbound(VpnMode mode);
    void ChangeMainOutbound(IVpnBound bound);
    
    bool IsSupportProtocol(VpnProtocol protocol);

    void SaveConfiguration();
    
    bool IsRunning { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}