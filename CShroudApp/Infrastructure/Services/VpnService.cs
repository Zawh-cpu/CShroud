using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    private readonly SettingsConfig _settingsConfig;
    
    private readonly IVpnCore _vpnCore;
    private readonly IApiRepository _apiRepository;
    private readonly IProxyManager _proxyManager;
    
    public bool IsRunning => _vpnCore.IsRunning;


    public VpnService(IOptions<SettingsConfig> settingsConfig,
        IVpnCore vpnCore, IApiRepository apiRepository, IProxyManager proxyManager)
    {
        _settingsConfig = settingsConfig.Value;
        _vpnCore = vpnCore;
        _apiRepository = apiRepository;
        _proxyManager = proxyManager;
    }
    

    public async Task EnableAsync(VpnMode mode)
    {
        var credentials = await _apiRepository.ConnectToVpnNetworkAsync(_vpnCore.SupportedProtocols, "de-frankfurt");
        if (credentials is null || !_vpnCore.IsSupportProtocol(credentials.Protocol))
        {
            throw new NotSupportedException($"{(credentials is null ? "UNKNOWN" : credentials.Protocol.ToString())} is unsupported protocol");
        }
        
        var outbound = IVpnBoundFactory.CreateFromCredentials(credentials);
        _vpnCore.ChangeMainOutbound(outbound);

        _vpnCore.ClearMainInbounds();
        if (mode == VpnMode.Proxy || mode == VpnMode.ProxyAndTun)
        {
            if (_vpnCore.IsSupportProtocol(VpnProtocol.Http))
            {
                var bound = new Http()
                {
                    Host = _settingsConfig.Network.Proxy.Http.Host,
                    Port = _settingsConfig.Network.Proxy.Http.Port,
                    Sniff = true,
                    SniffOverrideDestination = true
                };
                
                _vpnCore.AddInbound(bound);
            }

            if (_vpnCore.IsSupportProtocol(VpnProtocol.Socks))
            {
                var bound = new Socks()
                {
                    Host = _settingsConfig.Network.Proxy.Socks.Host,
                    Port = _settingsConfig.Network.Proxy.Socks.Port,
                    Sniff = true,
                    SniffOverrideDestination = true
                };
                
                _vpnCore.AddInbound(bound);
            }
        }

        if (mode == VpnMode.Tun || mode == VpnMode.ProxyAndTun)
        {
            // NEEDS TO IMPLEMENT
            if (_vpnCore.IsSupportProtocol(VpnProtocol.Tun))
            {
                
            }
            else
            {
                
            }
        }

        await _vpnCore.EnableAsync();
    }


    public Task DisableAsync()
    {
        throw new NotImplementedException();
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        VpnEnabled?.Invoke(sender, e);
    }

    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        VpnDisabled?.Invoke(sender, e);
    }
}