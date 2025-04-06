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
        var networkCredentials = await _apiRepository.ConnectToVpnNetworkAsync("de-frankfurt");
        
        _vpnCore.ChangeMainInbound(mode);
        _vpnCore.ChangeMainOutbound();
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