namespace CShroudApp.Core.Interfaces;

public interface IVpnCore
{
    bool IsRunning { get; }
    void Start();
    void Kill();
    
    event EventHandler VpnStopped;
    event EventHandler VpnStarted;
}