using CShroudApp.Core.Entities.Vpn.Bounds;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer
{
    private static JObject ParseVlessBound(Vless vless)
    {
        return new JObject()
        {
            ["type"] = "vless",
            ["tag"] = vless.Tag,
            ["server"] = vless.Host,
            ["server_port"] = vless.Port,
            ["uuid"] = vless.Uuid,
            ["flow"] = vless.Flow,
            ["package_encoding"] = vless.PackageEncoding,
            ["tls"] = new JObject()
            {
                ["enabled"] = true,
                ["server_name"] = vless.ServerName,
                ["insecure"] = vless.Insecure,
                ["utils"] = new JObject()
                {
                    ["enabled"] = true,
                    ["fingerprint"] = vless.Fingerprint,
                }
            },
            ["reality"] = new JObject()
            {
                ["enabled"] = true,
                ["public_key"] = vless.PublicKey,
                ["short_id"] = vless.ShortId
            }
        };
    }
}