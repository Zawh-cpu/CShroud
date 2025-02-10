using Newtonsoft.Json;
using System.Diagnostics;
using CShroudApp.Core.Domain.Entities;
using CShroudApp.Core.Domain.Generators;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class VpnService : IVpnService
{
    private static readonly string VPN_PATH = Core.BuildPath("Addons", "XrayCore", "xray.exe");

    private IVpnCore _vpnCore;
    private IProxyManager _proxyManager;
    private IServerRepository _serverRepository;
    
    public bool IsRunning => _vpnCore.IsRunning;
    
    public event EventHandler VpnStopped = delegate { };
    public event EventHandler VpnStarted = delegate { };

    public VpnService(IVpnCore vpnManager, IProxyManager proxyManager, IServerRepository serverRepository)
    {
        _vpnCore = vpnManager;
        _proxyManager = proxyManager;
        _serverRepository = serverRepository;

        string modifiedJson = JsonConvert.SerializeObject(SingBox.MakeDefaultConfig(), Formatting.Indented);
        File.WriteAllText("modified_config.json", modifiedJson);
    }

    public void Start()
    {
        //_serverRepository.MakeAVPNConnection();
        if (!_vpnCore.IsRunning)
        {
            _vpnCore.Start();
        }
        
        // _proxyManager.Enable("localhost:7000");
    }

    public void Stop()
    {
        _proxyManager.Disable();
    }

    public void UpdateConfig()
    {
        if (_serverRepository != null) {}
        //return false;
    }

}