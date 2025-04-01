namespace CShroudApp.Infrastructure.Data.Config;

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