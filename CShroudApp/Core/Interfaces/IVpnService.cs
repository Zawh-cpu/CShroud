using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Core.Interfaces;

public interface IVpnService
{
    Task Enable(VpnMode mode);
    void Disable();
    void IsEnabled();
    
    bool IsRunning();
    
    event EventHandler OnVpnEnabled;
    event EventHandler OnVpnDisabled;
}