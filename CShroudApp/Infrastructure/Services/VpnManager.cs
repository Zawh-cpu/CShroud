using Newtonsoft.Json;
using System.Diagnostics;
using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnManager : IVpnManager
{
    private static readonly string VPN_PATH = Core.BuildPath("Addons", "XrayCore", "xray.exe");
    private readonly BaseProcess _process;

    bool IsRunning => _process.IsRunning;

    private IVpnCore _vpnCore;
    private IProxyManager _proxyManager;

    public VpnManager(IVpnCore vpnManager, IProxyManager proxyManager)
    {
        _vpnCore = vpnManager;
        _proxyManager = proxyManager;

        string modifiedJson = JsonConvert.SerializeObject(CreateDefaultConfig(), Formatting.Indented);
        File.WriteAllText("modified_config.json", modifiedJson);
    }

    public void Start()
    {
        _process.Start();
        _process.ProcessStarted += ProcessSuccessfullyStarted;
        Console.WriteLine("Started VPN process");
    }

    private void ProcessSuccessfullyStarted(object? sender, EventArgs e)
    {
        Console.WriteLine("start proxy");
    }

    public void Stop()
    {
        _process.Kill();
    }

    public void UpdateConfig()
    {
    }

    /*
     public static Config CreateDefaultConfig()
    {
        return new Config
        {
            Log = new Log
            {
                Access = "",
                Error = "",
                LogLevel = "warning"
            },
            Inbounds = new List<Inbound>
            {
                new Inbound
                {
                    Tag = "socks",
                    Port = 10800,
                    Listen = "127.0.0.1",
                    Protocol = "socks",
                    Sniffing = new Sniffing { Enabled = true, DestOverride = new List<string> { "http", "tls" }, RouteOnly = false },
                    Settings = new Settings { Auth = "noauth", Udp = true, AllowTransparent = false }
                },
                new Inbound
                {
                    Tag = "http",
                    Port = 10801,
                    Listen = "127.0.0.1",
                    Protocol = "http",
                    Sniffing = new Sniffing { Enabled = true, DestOverride = new List<string> { "http", "tls" }, RouteOnly = false },
                    Settings = new Settings { Auth = "noauth", Udp = true, AllowTransparent = false }
                }
            },
            Outbounds = new List<Outbound>
            {
                new Outbound
                {
                    Tag = "proxy",
                    Protocol = "vless",
                    Settings = new { vnext = new List<Vnext> { new Vnext { Address = "frankfurt.reality.zawh.ru", Port = 443, Users = new List<User> { new User { Id = "8d50da4e-fff4-4188-bbd1-7d620c7296f0", AlterId = 0, Email = "t@t.tt", Security = "auto", Encryption = "none", Flow = "xtls-rprx-vision" } } } } },
                    StreamSettings = new StreamSettings { Network = "tcp", Security = "reality", RealitySettings = new RealitySettings { ServerName = "google.com", Fingerprint = "random", Show = false, PublicKey = "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI", ShortId = "4ae60b64b5cd", SpiderX = "/" } },
                    Mux = new Mux { Enabled = false, Concurrency = -1 }
                },
                new Outbound { Tag = "direct", Protocol = "freedom", Settings = new { } },
                new Outbound { Tag = "block", Protocol = "blackhole", Settings = new { response = new { type = "http" } } }
            },
            Dns = new Dns
            {
                Hosts = new Dictionary<string, string>
                {
                    { "dns.google", "8.8.8.8" },
                    { "proxy.example.com", "127.0.0.1" }
                },
                Servers = new List<object> { "223.5.5.5", "1.1.1.1", "8.8.8.8", "https://dns.google/dns-query" }
            },
            Routing = new Routing
            {
                DomainStrategy = "AsIs",
                Rules = new List<Rule>
                {
                    new Rule { Type = "field", InboundTag = new List<string> { "api" }, OutboundTag = "api" },
                    new Rule { Type = "field", OutboundTag = "direct", Domain = new List<string> { "domain:example-example.com", "domain:example-example2.com" } },
                    new Rule { Type = "field", Port = "443", Network = "udp", OutboundTag = "block" },
                    new Rule { Type = "field", OutboundTag = "block", Domain = new List<string> { "geosite:category-ads-all" } },
                    new Rule { Type = "field", OutboundTag = "direct", Domain = new List<string> { "domain:dns.alidns.com", "domain:doh.pub", "domain:dot.pub", "domain:doh.360.cn", "domain:dot.360.cn", "geosite:cn", "geosite:geolocation-cn" } },
                    new Rule { Type = "field", OutboundTag = "direct", Ip = new List<string> { "223.5.5.5/32", "223.6.6.6/32", "2400:3200::1/128", "2400:3200:baba::1/128" } },
                    new Rule { Type = "field", Port = "0-65535", OutboundTag = "proxy" }
                }
            }
        };
    }

}
*/

    public static Dictionary<string, object> CreateDefaultConfig()
    {
        Dictionary<string, object> dict = new();

        dict["log"] = new Dictionary<string, object>()
        {
            {"access", ""},
            {"error", ""},
            {"loglevel", "warning"}
        };

        dict["inbounds"] = new List<object>();
        dict["outbounds"] = new List<object>();
        
        dict["dns"] = new Dictionary<string, object>();
        dict["routing"] = new Dictionary<string, object>();
        
        return dict;
    }
}