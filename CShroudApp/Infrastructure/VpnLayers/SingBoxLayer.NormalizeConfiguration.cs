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
                    ["address"] = "8.8.8.8",
                    // Delete this line if some an errors occurs
                    ["strategy"] = "ipv4_only",
                    ["detour"] = "proxy"
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
                new JObject()
                {
                    ["server"] = "remote",
                    ["clash_mode"] = "Global",
                }
            },
            ["final"] = "remote",
        };

        _configuration["inbounds"] ??= new JArray();
        _configuration["outbounds"] ??= new JArray();
        _configuration["expiremental"] ??= new JObject()
        {
            ["cache_file"] = new JObject()
            {
                ["enabled"] = "true",
            },
            /*["clash_api"] = new JObject()
            {
                ["external_controller"] = "127.0.0.1:10814",
            }*/
        };
    }
}