using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;

namespace CShroudApp.Core.Interfaces;

public interface IVpnCoreLayer
{
    public bool IsProtocolSupported(VpnProtocol protocol);
    
    public void AddInbound(IVpnBound bound, int index = int.MaxValue);
    public void AddOutbound(IVpnBound bound, int index = int.MaxValue);
    public void RemoveInbound(string tag, bool startsWithMode = false);
    public void RemoveOutbound(string tag, bool startsWithMode = false);

    public void StartProcess();
    public Task KillProcessAsync();
    
    public List<VpnProtocol> SupportedProtocols { get; }

    public void FixDnsIssues(List<string> transparentHosts);
    
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    
    public bool IsRunning { get; }

    public void SaveConfiguration();
}