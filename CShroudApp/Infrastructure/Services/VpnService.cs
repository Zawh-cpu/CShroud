using System.Reflection;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    private readonly IVpnCore _vpnCore;
    private readonly IApiRepository _apiRepository;

    public VpnService(IVpnCore vpnCore, IApiRepository apiRepository)
    {
        _vpnCore = vpnCore;
        _apiRepository = apiRepository;
    }
    
    public async Task Enable(VpnMode mode)
    {
        if (_vpnCore.IsRunning) await _vpnCore.Disable();
        
        var networkCredentials = await _apiRepository.ConnectToVpnNetworkAsync("de-frankfurt");
        // Needs to check for support

        var outbound = IVpnBoundFactory.CreateFromCredentials(networkCredentials);
        
        // REFLECTION
        Type type = outbound.GetType();
        PropertyInfo? propertyInfo;
        propertyInfo = type.GetProperty("Fingerprint", BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(outbound, "random");
        }
        
        propertyInfo = type.GetProperty("PackageEncoding", BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(outbound, "xudp");
        }
        
        _vpnCore.ChangeMainInbound(mode);
        _vpnCore.ChangeMainOutbound(outbound);
        _vpnCore.SaveConfiguration();
        
        // _vpnCore.Enable();

        if (!_vpnCore.IsSupportProtocol(VpnProtocol.Tun))
        {
            Console.WriteLine("Needs to enable tun");
        }
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }

    public void IsEnabled()
    {
        throw new NotImplementedException();
    }

    public bool IsRunning()
    {
        throw new NotImplementedException();
    }

    public event EventHandler? OnVpnEnabled;
    public event EventHandler? OnVpnDisabled;
}