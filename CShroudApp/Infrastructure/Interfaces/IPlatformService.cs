using System.Runtime.InteropServices;

namespace CShroudApp.Infrastructure.Interfaces;

public interface IPlatformService
{
    
    public OSPlatform Platform { get; }
    public bool IsPlatformSupported { get; }
}