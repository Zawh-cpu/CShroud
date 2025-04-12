using System.Reflection;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    private readonly IVpnCore _vpnCore;
    private readonly IApiRepository _apiRepository;
    public bool IsRunning => _vpnCore.IsRunning;
    
    public event EventHandler? VpnEnabled;
    public event EventHandler? VpnDisabled;
    
    private VpnMode _currentVpnMode = VpnMode.Disabled;
    private readonly IProxyManager _proxyManager;
    
    private SettingsConfig _settingsConfig;
    
    public VpnService(IVpnCore vpnCore, IApiRepository apiRepository, IProxyManager proxyManager, IOptions<SettingsConfig> settingsConfig)
    {
        _vpnCore = vpnCore;
        _apiRepository = apiRepository;
        _proxyManager = proxyManager;
        _settingsConfig = settingsConfig.Value;
        
        _vpnCore.VpnDisabled += OnVpnDisabled;
        _vpnCore.VpnEnabled += OnVpnEnabled;
    }

    public async Task UpdateCredentials()
    {
        
    }
    
    public async Task EnableAsync(VpnMode mode)
    {
        if (_vpnCore.IsRunning) await _vpnCore.DisableAsync();
        
        _currentVpnMode = mode;
        
        var networkCredentials = await _apiRepository.ConnectToVpnNetworkAsync(_vpnCore.SupportedProtocols, "de-frankfurt");
        if (!_vpnCore.IsSupportProtocol(networkCredentials.Protocol))
        {
            throw new InvalidOperationException("VPN protocol is not supported");
        }

        var outbound = IVpnBoundFactory.CreateFromCredentials(networkCredentials);
        
        // REFLECTION
        Type type = outbound.GetType();
        PropertyInfo? propertyInfo;
        propertyInfo = type.GetProperty("Fingerprint", BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(outbound, "random");
        }
        
        propertyInfo = type.GetProperty("PacketEncoding", BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(outbound, "xudp");
        }
        
        _vpnCore.ClearMainInbound();
        if (mode == VpnMode.Proxy || mode == VpnMode.ProxyAndTun)
        {
            // var socksInboundDa
            var socksInbound = new Socks()
            {
                Tag = "main-net-socks",
                Host = "127.0.0.1",
                Port = 10808,
                Sniff = true,
                SniffOverrideDestination = true
            };
        
            var httpInbound = new Http()
            {
                Tag = "main-net-http",
                Host = "127.0.0.1",
                Port = 10809,
                Sniff = true,
                SniffOverrideDestination = true
            };
            
            _vpnCore.AddInbound(socksInbound);
            _vpnCore.AddInbound(httpInbound);
        }
        
        _vpnCore.ChangeMainOutbound(outbound);
        _vpnCore.FixDnsIssues(networkCredentials.TransparentHosts);
        _vpnCore.SaveConfiguration();
        
        await _vpnCore.EnableAsync();
    }

    public async Task DisableAsync()
    {
        await _vpnCore.DisableAsync();
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        // Enable proxy/vpn interfaces
        if (_currentVpnMode == VpnMode.Proxy || _currentVpnMode == VpnMode.ProxyAndTun)
        {
            // ENABLE PROXY VIA PROXY_MANAGER
            Console.WriteLine("[VPN->ENABLED] >>> PROXY->ON");
            _proxyManager.EnableAsync(_settingsConfig.Network.Proxy.Http, new List<string>()).GetAwaiter().GetResult();
        }

        if (_currentVpnMode == VpnMode.Tun || _currentVpnMode == VpnMode.ProxyAndTun)
        {
            // ENABLE TUN VIA TUN_MANAGER
            Console.WriteLine("[VPN->ENABLED] >>> TUN->ON");
        }
        
        VpnEnabled?.Invoke(sender, e);
    }
    
    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        // Disable proxy/vpn interfaces
        _currentVpnMode = VpnMode.Disabled;
        VpnDisabled?.Invoke(sender, e);
    }
}