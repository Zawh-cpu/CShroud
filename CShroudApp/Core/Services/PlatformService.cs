using System.Runtime.InteropServices;


namespace CShroudApp.Core.Services;

public enum Platform
{
    Windows,
    Linux,
    OSX,
    FreeBSD,
}


public static class PlatformService
{
    public static readonly Platform Platform = _GetPlatform();

    public static HashSet<Platform> SupportedPlatforms { get; } =
    [
        Platform.Windows,
        Platform.Linux
    ];

    public static readonly bool IsPlatformSupported = _IsPlatformSupported(Platform);

    private static Platform _GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return Platform.Windows;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return Platform.Linux;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return Platform.OSX;
        return Platform.FreeBSD;
    }

    private static bool _IsPlatformSupported(Platform platform)
    {
        return SupportedPlatforms.Contains(platform);
    }
}