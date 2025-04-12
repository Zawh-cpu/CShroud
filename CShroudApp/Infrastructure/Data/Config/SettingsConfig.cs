using CShroudApp.Core.Entities.Vpn;
using Microsoft.Extensions.Configuration;

namespace CShroudApp.Infrastructure.Data.Config;

public enum DebugMode
{
    None,
    Debug,
    Info,
    Warning,
    Error
}

public enum VpnCore
{
    SingBox,
    Xray
}

public class SettingsConfig
{
    public DebugMode DebugMode { get; set; } = DebugMode.None;
    public NetworkObject Network { get; set; } = new();

    public class NetworkObject
    {
        public VpnMode Mode { get; set; } = VpnMode.ProxyAndTun;
        public VpnCore Core { get; set; } = VpnCore.SingBox;
        public ProxyObject Proxy { get; set; } = new();

        public class ProxyObject
        {
            public ProxyData Http { get; set; } = new() { Host = "127.0.0.1", Port = 11808 };
            public ProxyData Socks { get; set; } = new() { Host = "127.0.0.1", Port = 11809 };

            public class ProxyData
            {
                public string Host { get; set; } = "127.0.0.1";
                public uint Port { get; set; } = 1000;
            }
        }
    }
}