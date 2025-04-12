using System.Diagnostics;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Data.Json.Policies;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer : IVpnCoreLayer
{
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    public List<VpnProtocol> SupportedProtocols { get; } = new()
    {
        VpnProtocol.Vless, VpnProtocol.Http, VpnProtocol.Socks, VpnProtocol.Tun
    };
    
    public bool IsProtocolSupported(VpnProtocol protocol) => SupportedProtocols.Contains(protocol);

    private readonly BaseProcess _process;
    
    private SingBoxConfig _configuration = new();
    
    private readonly Dictionary<VpnProtocol, Func<IVpnBound, SingBoxConfig.BoundObject>> _vpnProtocolsHandlers = new()
    {
        { VpnProtocol.Vless, inbound => ParseVlessBound((Vless)inbound) },
        { VpnProtocol.Http, inbound => ParseHttpBound((Http)inbound) },
        { VpnProtocol.Socks, inbound => ParseSocksBound((Socks)inbound) },
    };
    
    public SingBoxLayer(IProcessManager processManager, IOptions<SettingsConfig> settings)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.CurrentDirectory, "Binaries", "Cores", "SingBox", "sing-box.exe"),
            Arguments = "run -c stdin",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        _process = new BaseProcess(processStartInfo, settings.Value.DebugMode);
        _process.ProcessExited += OnProcessExited;
        _process.ProcessStarted += OnProcessStarted;
        
        processManager.Register(_process);
    }

    public void AddInbound(IVpnBound bound, int index = Int32.MaxValue)
    {
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        if (index < 0 || index > _configuration.Inbounds.Count) _configuration.Inbounds.Add(action(bound));
        else _configuration.Inbounds.Insert(index, action(bound));
    }

    public void AddOutbound(IVpnBound bound, int index = Int32.MaxValue)
    {
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        if (index < 0 || index > _configuration.Outbounds.Count) _configuration.Outbounds.Add(action(bound));
        else _configuration.Outbounds.Insert(index, action(bound));
    }

    public void RemoveInbound(string tag, bool startsWithMode = false)
    {
        if (startsWithMode) _configuration.Inbounds.RemoveAll(x => x.Tag.StartsWith(tag));
        else _configuration.Inbounds.RemoveAll(x => x.Tag == tag);
    }

    public void RemoveOutbound(string tag, bool startsWithMode = false)
    {
        if (startsWithMode) _configuration.Outbounds.RemoveAll(x => x.Tag.StartsWith(tag));
        else _configuration.Outbounds.RemoveAll(x => x.Tag == tag);
    }

    public async Task StartProcessAsync()
    {
        if (IsRunning) return;

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        var x = JsonConvert.SerializeObject(_configuration, settings);
        
        Console.WriteLine(x);
        
        _process.Start();
        await _process.StandardInput.WriteAsync(x);
        _process.StandardInput.Close();
    }

    public void ConcatConfigs(SettingsConfig settings)
    {
        _configuration.Log.Disabled = settings.DebugMode == DebugMode.None ? true : false;
        _configuration.Log.Level = settings.DebugMode.ToString().ToLowerInvariant();
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "remote",
            ["address"] = "1.1.1.1",
            ["detour"] = "main-net-outbound"
        });
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "block",
            ["address"] = "rcode://success"
        });
        
        _configuration.Dns.Servers.Add(new JObject()
        {
            ["tag"] = "local",
            ["address"] = "1.1.1.1",
            ["detour"] = "direct"
        });

        _configuration.Dns.Final = "remote";
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "direct",
            Tag = "direct"
        });
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "block",
            Tag = "block"
        });
        
        _configuration.Outbounds.Add(new SingBoxConfig.BoundObject()
        {
            Type = "dns",
            Tag = "dns_out"
        });
        
        _configuration.Route.Rules.Add(new JObject()
        {
            ["outbound"] = "dns_out",
            ["protocol"] = new JArray() { "dns" }
        });
        
        _configuration.Route.Rules.Add(new JObject()
        {
            ["outbound"] = "main-net-outbound",
            ["port_range"] = "0:65535"
        });
    }
    
    public async Task KillProcessAsync()
    {
        if (!IsRunning) return;
        await _process.KillAsync();
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        _configuration.Dns.Rules.Add(new JObject()
        {
            ["server"] = "local",
            ["domain"] = JArray.FromObject(transparentHosts)
        });
    }

    public bool IsRunning => _process.IsRunning;

    private void OnProcessExited(object? sender, EventArgs e)
    {
        ProcessExited?.Invoke(this, e);
    }

    private void OnProcessStarted(object? sender, EventArgs e)
    {
        ProcessStarted?.Invoke(this, e);
    }
}