using CShroudApp.Core.Entities.Vpn.Bounds;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer
{
    private static JObject ParseSocksBound(Socks data)
    {
        return new JObject()
        {
            ["type"] = "socks",
            ["tag"] = data.Tag,
            ["listen"] = data.Host,
            ["listen_port"] = data.Port,
            ["sniff"] = data.Sniff,
            ["sniff_override_destination"] = data.SniffOverrideDestination,
        };
    }
}