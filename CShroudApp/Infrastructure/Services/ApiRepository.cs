using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class ApiRepository : IApiRepository
{
    async public Task<VpnNetworkCredentials> ConnectToVpnNetworkAsync(string location)
    {
        return new VpnNetworkCredentials()
        {
            ServerHost = "localhost",
            ServerPort = 443,
            IPv4 = "127.0.0.1",
            IPv6 = "::1",
            Location = location,
            Obtained = DateTime.UtcNow,
            Protocol = VpnProtocol.Vless,
            Credentials = new Dictionary<string, object>()
        };
    }
}