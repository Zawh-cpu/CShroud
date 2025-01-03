namespace CShroud.Infrastructure.Interfaces;

public interface IVpnCore
{
    bool IsRunning { get; }
    void Start();
}