using Newtonsoft.Json;
using System.Diagnostics;
using CShroudApp.Core.Domain.Entities;
using CShroudApp.Core.Domain.Generators;
using CShroudApp.Infrastructure.Interfaces;
using System.Linq;

namespace CShroudApp.Infrastructure.Services;

enum VpnMode
{
    Proxy,
    Tun,
    ProxyAndTun
}

public class VpnService : IVpnService
{
    private IVpnCore _vpnCore;
    private VpnCoreConfig _vpnConfig;
    private IProxyManager _proxyManager;
    private IServerRepository _serverRepository;
    
    public bool IsRunning => _vpnCore.IsRunning;
    
    public event EventHandler VpnStopped = delegate { };
    public event EventHandler VpnStarted = delegate { };
    public event EventHandler VpnFailed = delegate { };

    public VpnService(IVpnCore vpnManager, VpnCoreConfig vpnConfig, IProxyManager proxyManager, IServerRepository serverRepository)
    {
        _vpnCore = vpnManager;
        _vpnConfig = vpnConfig;
        _proxyManager = proxyManager;
        _serverRepository = serverRepository;

        // string modifiedJson = JsonConvert.SerializeObject(SingBox.MakeDefaultConfig(), Formatting.Indented);
        // File.WriteAllText("modified_config.json", modifiedJson);
    }

    public void Start(VpnMode mode = VpnMode.Proxy)
    {
        if (!_serverRepository.MakeAVpnConnection("frankfurt", "vless", out ConnectionAnswer answer))
        {
            VpnFailed.Invoke(this, EventArgs.Empty);
            return;
        }
        
        UpdateConfig(answer, mode);
        
        if (!_vpnCore.IsRunning)
        {
            _vpnCore.Start();
        }
        
        // _proxyManager.Enable("localhost:7000");
        
        VpnStarted.Invoke(this, EventArgs.Empty);
    }

    public void Stop()
    {
        _proxyManager.Disable();
        VpnStopped.Invoke(this, EventArgs.Empty);
    }

    public void UpdateConfig(ConnectionAnswer answer, VpnMode mode)
    {
        var config = GetAndRestoreConfig();
        
        config["outbounds"] = (config["outbounds"] as Newtonsoft.Json.Linq.JArray)!.ToObject<List<Dictionary<string, object>>>()!
            .Where(x => x.GetValueOrDefault("type", string.Empty).ToString() != answer.Protocol).ToList();
        
        (config["outbounds"] as List<Dictionary<string, object>>)!.Insert(0, answer.Data);

        var inbounds = new List<Dictionary<string, object>>();
        if (mode == VpnMode.Proxy || mode == VpnMode.ProxyAndTun)
        {
            inbounds.Add(SingBox.MakeInboundSocks(),  SingBox.MakeInboundHTTP());
        }

        if (mode == VpnMode.Tun || mode == VpnMode.ProxyAndTun)
        {
            inbounds.Add(SingBox.MakeInboundTunConfig());
        }
        
        config["inbounds"] = inbounds;
        
        File.WriteAllText(_vpnConfig.ConfigPath, JsonConvert.SerializeObject(config, Formatting.Indented));
    }

    public Dictionary<string, object> GetAndRestoreConfig()
    {
        Dictionary<string, object> config;
        
        if (!File.Exists(_vpnConfig.ConfigPath))
        {
            config = SingBox.MakeDefaultConfig();
        }
        else
        {
            config = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_vpnConfig.ConfigPath))!;
        }
        
        SingBox.CheckAndRestoreConfig(config);
        
        return config;
    }
    
    public void SwitchTunMode(bool status)
    {
        if (_vpnCore.IsRunning)
        {
            _vpnCore.Kill();
        }

        var config = GetAndRestoreConfig();
        config["inbounds"] = (config["outbounds"] as Newtonsoft.Json.Linq.JArray)!.ToObject<List<Dictionary<string, object>>>()!
            .Where(x => x.GetValueOrDefault("type", string.Empty).ToString() != "tun").ToList();
        
        if (status)
        {
            (config["outbounds"] as List<Dictionary<string, object>>)!.Insert(0, SingBox.MakeInboundTunConfig());
        }
        
        config["outbounds"] = "true";
        
        throw new NotImplementedException();
    }

}