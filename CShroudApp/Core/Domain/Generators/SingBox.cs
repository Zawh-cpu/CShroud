namespace CShroudApp.Core.Domain.Generators;

public class SingBox
{
    public static Dictionary<string, object> MakeDefaultConfig()
    {
        var result = new Dictionary<string, object>();
        
        result["dns"] = new List<Dictionary<string, object>>()
        {
            new()
            {
                {"tag", "block"},
                {"address", "rcode://success"}
            }
        };

        result["inbounds"] = new List<Dictionary<string, object>>();
        result["outbounds"] = new List<Dictionary<string, object>>();
        
        result["route"] = new Dictionary<string, object>()
        {
            {"auto_detect_interface", true},
            { "rules", new List<Dictionary<string, object>>() }
        };
        
        return result;
    }

    public static void CheckAndRestoreConfig(Dictionary<string, object> config)
    {
        // config["dns"] = config.GetValueOrDefault("dns", new Dictionary<string, object>());
        config["outbounds"] = config.GetValueOrDefault("outbounds", new List<Dictionary<string, object>>());
    }

    public static Dictionary<string, object> MakeInboundTunConfig()
    {
        return new()
        {
            { "type", "tun" },
            { "tag", "tun-in" },
            { "interface_name", "CrimsonShroud TUN-Adapter" },
            { "address", new List<string>() { "172.19.0.1/30"} },
            { "mtu", 9000 },
            { "auto_route", true },
            { "strict_route", true },
            { "stack", "gvisor" },
            { "sniff", true },
        };
    }
    
    public static Dictionary<string, object> MakeInboundSocks(int port = 10808)
    {
        return new()
        {
            { "type", "socks" },
            { "tag", "socks-in" },
            { "listen", "127.0.0.1" },
            { "listen_port", port },
            { "sniff", true },
            { "sniff_override_destination", true }
        };
    }
    
    public static Dictionary<string, object> MakeInboundHttp(int port = 10809)
    {
        return new()
        {
            { "type", "http" },
            { "tag", "http-in" },
            { "listen", "127.0.0.1" },
            { "listen_port", port },
            { "sniff", true },
            { "sniff_override_destination", true }
        };
    }
}