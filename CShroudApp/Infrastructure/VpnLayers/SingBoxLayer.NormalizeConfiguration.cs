using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer
{
    private void NormalizeConfiguration()
    {
        _configuration["dns"] ??= new JObject()
        {
            ["servers"] = new JArray()
            {
                new JObject()
                {
                    ["tag"] = "remote",
                    ["address"] = "1.1.1.1",
                    // ["strategy"] = "ipv4_only",
                    ["detour"] = "main-net-outbound"
                },
                new JObject()
                {
                    ["tag"] = "cloudflare_dns",
                    ["address"] = "1.1.1.1",
                    ["detour"] = "direct",
                },
                new JObject()
                {
                    ["tag"] = "google_dns",
                    ["address"] = "8.8.8.8",
                    ["detour"] = "direct",
                },
                new JObject()
                {
                    ["tag"] = "alibaba_dns",
                    ["address"] = "223.5.5.5",
                    ["detour"] = "direct",
                },
                new JObject()
                {
                    ["tag"] = "block",
                    ["address"] = "rcode://success",
                }
            },
            // Maybe a trash...
            ["rules"] = new JArray()
            {
                /*new JObject()
                {
                    ["server"] = "alibaba_dns",
                    ["domain"] = new JArray(),
                    ["rule_set"] = new JArray() { "geosite-geolocation-cn", "geosite-cn" }
                },*/
                new JObject()
                {
                    ["server"] = "cloudflare_dns",
                    ["domain"] = new JArray()
                }
            },
            ["final"] = "remote",
        };

        _configuration["inbounds"] ??= new JArray();
        _configuration["outbounds"] ??= new JArray()
        {
            new JObject()
            {
                ["type"] = "direct",
                ["tag"] = "direct"
            },
            new JObject()
            {
                ["type"] = "block",
                ["tag"] = "block"
            },
            new JObject()
            {
                ["type"] = "dns",
                ["tag"] = "dns_out"
            },
        };
        
        _configuration["route"] ??= new JObject()
        {
            ["rules"] = new JArray()
            {
                new JObject()
                {
                    ["outbound"] = "dns_out",
                    ["protocol"] = new JArray() { "dns" }
                },
                new JObject()
                {
                    ["outbound"] = "main-net-outbound",
                    ["port_range"] = new JArray()
                    {
                        "0:65535"
                    }
                }
            }
        };
        
        _configuration["experimental"] ??= new JObject()
        {
            ["cache_file"] = new JObject()
            {
                ["enabled"] = true,
            },
            /*["clash_api"] = new JObject()
            {
                ["external_controller"] = "127.0.0.1:10814",
            }*/
        };
    }
}