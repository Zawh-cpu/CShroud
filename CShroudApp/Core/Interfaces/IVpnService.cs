using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Core.Interfaces;

public interface IVpnService
{
    void Start(VpnMode mode);
    void Stop();
}