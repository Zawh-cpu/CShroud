namespace CShroudApp.Core.Domain.Entities;

public enum VpnMode
{
    Proxy,
    Tun,
    ProxyAndTun
}

public class ApplicationConfig
{
    public required string WorkingFolder;
    public required SettingsConfig Settings;
}

public class SettingsConfig
{
    public required VpnMode VpnMode;
}