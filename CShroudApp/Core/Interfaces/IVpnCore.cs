using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    Task EnableAsync();
    Task DisableAsync();
    
    void ClearMainInbounds();
    void AddInbound(IVpnBound bound);
    void ChangeMainOutbound(IVpnBound bound);
    
    bool IsSupportProtocol(VpnProtocol protocol);

    void FixDnsIssues(List<string> transparentHosts);
    
    bool IsRunning { get; }
    
    List<VpnProtocol> SupportedProtocols { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
}