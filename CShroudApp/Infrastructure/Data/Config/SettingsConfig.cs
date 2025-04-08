using CShroudApp.Core.Entities.Vpn;

namespace CShroudApp.Infrastructure.Data.Config;

public enum DebugType
{
    None,
    Debug,
    Info,
    Warning,
    Error
}

public class SettingsConfig
{
    public DebugType Debug { get; set; } = DebugType.None;
    public SettingsNetworkConfig Network { get; set; } = new();
}

public class SettingsNetworkConfig
{
    public VpnMode Mode { get; set; } = VpnMode.Proxy;
    public VpnCore Core { get; set; } = VpnCore.SingBox;
    public SettingsNetworkSplitTunneling SplitTunneling = new();
}

public enum VpnCore
{
    SingBox,
    Xray
}

public class SettingsNetworkSplitTunneling
{
    public bool Enabled { get; set; } = true;
    public string Mode { get; set; } = "Blacklist";
    public List<string> AllowedApplications { get; set; } = new();
}