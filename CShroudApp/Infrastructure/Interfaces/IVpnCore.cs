namespace CShroudApp.Infrastructure.Interfaces;

public interface IVpnCore
{
    bool IsRunning { get; }
    void Start();
    void Kill();
    
    event EventHandler VpnStopped;
    event EventHandler VpnStarted;
}