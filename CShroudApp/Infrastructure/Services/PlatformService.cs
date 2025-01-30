using System.Runtime.InteropServices;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class PlatformService : IPlatformService
{
    public OSPlatform Platform { get; }

    public bool IsPlatformSupported { get; }

    private readonly HashSet<OSPlatform> _supportedPlatforms =
    [
        OSPlatform.Windows,
        OSPlatform.Linux
    ];
    
    public PlatformService()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Platform = OSPlatform.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) Platform = OSPlatform.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Platform = OSPlatform.OSX;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) Platform = OSPlatform.FreeBSD;
        
        IsPlatformSupported = _supportedPlatforms.Contains(Platform);
    }
}