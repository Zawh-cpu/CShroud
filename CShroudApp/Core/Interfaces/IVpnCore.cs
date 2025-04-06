using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    void Enable();
    void Disable();
    
    void ChangeMainInbound(VpnMode mode);
    void ChangeMainOutbound(IVpnBound bound);
    
    bool IsSupportProtocol(VpnProtocol protocol);

    void SaveConfiguration();
    
    bool IsRunning();
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}