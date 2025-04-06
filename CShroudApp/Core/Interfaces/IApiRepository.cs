using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Core.Interfaces;

public interface IApiRepository
{
    Task<VpnNetworkCredentials> ConnectToVpnNetworkAsync(string location);
}