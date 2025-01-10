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
    
    public bool IsRunning => _vpnCore.IsRunning;

    public VpnService(IVpnCore vpnManager, IProxyManager proxyManager)
    {
        _vpnCore = vpnManager;
        _proxyManager = proxyManager;

        string modifiedJson = JsonConvert.SerializeObject(SingBox.MakeDefaultConfig(), Formatting.Indented);
        File.WriteAllText("modified_config.json", modifiedJson);
    }

    public void Start() {}
    public void Stop() {}
    public void UpdateConfig() {}

}