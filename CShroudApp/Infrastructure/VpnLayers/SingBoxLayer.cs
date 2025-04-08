using System.Diagnostics;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer : IVpnCoreLayer
{
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    
    private readonly IProcessManager _processManager;
    private readonly PathConfig _pathConfig;
    private readonly SettingsConfig  _settingsConfig;
    
    private readonly List<VpnProtocol> _vpnSupportedProtocols = [VpnProtocol.Vless, VpnProtocol.Socks, VpnProtocol.Http];
    private readonly List<VpnProtocol> _vpnSupportedConnectionProtocols = [VpnProtocol.Vless];
    private readonly Dictionary<VpnProtocol, Func<IVpnBound, JObject>> _vpnProtocolsHandlers = new()
    {
        { VpnProtocol.Vless, inbound => ParseVlessBound((Vless)inbound) },
        { VpnProtocol.Http, inbound => ParseHttpBound((Http)inbound) },
        { VpnProtocol.Socks, inbound => ParseSocksBound((Socks)inbound) },
    };
    
    public List<VpnProtocol> SupportedProtocols => _vpnSupportedConnectionProtocols;
    
    private JObject _configuration;
    private readonly BaseProcess _process;
    public bool IsRunning => _process.IsRunning;

    private readonly string _configurationPath;

    private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented
    };

    public SingBoxLayer(IProcessManager processManager, IOptions<PathConfig> pathConfig, IOptions<SettingsConfig> settingsConfig)
    {
        _processManager = processManager;
        _pathConfig = pathConfig.Value;
        _settingsConfig = settingsConfig.Value;
        
        _configurationPath = Path.Combine(Environment.CurrentDirectory, _pathConfig.Cores.SingBox.Path, "config.json");
        _configuration = JObject.Parse(File.ReadAllText(_configurationPath));

        NormalizeConfiguration();
        _configuration["log"] = new JObject()
        {
            ["disabled"] = _settingsConfig.Debug != DebugType.None,
            ["level"] = (_settingsConfig.Debug == DebugType.None ? DebugType.Error : _settingsConfig.Debug).ToString().ToLower(),
            ["timestamp"] = true
        };
        SaveConfiguration();

        var workingPath = Path.Combine(Environment.CurrentDirectory, _pathConfig.Cores.SingBox.Path);
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(workingPath, "sing-box.exe"),
            Arguments = _pathConfig.Cores.SingBox.Args + $" -D {workingPath}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        _process = new BaseProcess(processStartInfo, debug: _settingsConfig.Debug);
        
        _process.ProcessExited += OnProcessExited;
        _process.ProcessStarted += OnProcessStarted;
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        var array = (JArray)_configuration["dns"]!["rules"]!;
        var dnsRules = array.Where(x => x.Type == JTokenType.Object && (x.Value<string>("server") ?? "").EndsWith("_dns")).ToList();
        foreach (var rule in dnsRules)
        {
            if (rule.Type != JTokenType.Object) continue;
            ((JObject)rule!)["domain"] = JArray.FromObject(transparentHosts);
        }
    }
    
    public void SaveConfiguration()
    {
        File.WriteAllText(_configurationPath, JsonConvert.SerializeObject(_configuration, _serializerSettings));
    }

    public bool IsProtocolSupported(VpnProtocol protocol)
    {
        return _vpnSupportedProtocols.Contains(protocol);
    }

    public void AddInbound(IVpnBound bound, int index = int.MaxValue)
    {
        if (!IsProtocolSupported(bound.Type)) throw new NotSupportedException();
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        var array = (JArray)_configuration["inbounds"]!;
        
        if (index < 0 || index > array.Count) array.Add(action(bound));
        else array.Insert(index, action(bound));
    }

    public void AddOutbound(IVpnBound bound, int index = int.MaxValue)
    {
        if (!IsProtocolSupported(bound.Type)) throw new NotSupportedException();
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();

        var array = (JArray)_configuration["outbounds"]!;
        
        if (index < 0 || index > array.Count) array.Add(action(bound));
        else array.Insert(index, action(bound));
    }

    public void RemoveInbound(string tag, bool startsWithMode = false)
    {
        IEnumerable<JToken> query;
        if (startsWithMode)
        {
            query = _configuration["inbounds"]!.Where(elem => ((string)elem["tag"]!).StartsWith(tag));
        }
        else
        {
            query = _configuration["inbounds"]!.Where(elem => (string?)elem["tag"] == tag);
        }
        
        query.ToList().ForEach(elem => elem.Remove());
    }

    public void RemoveOutbound(string tag, bool startsWithMode = false)
    {
        IEnumerable<JToken> query;
        if (startsWithMode)
        {
            query = _configuration["outbounds"]!.Where(elem => ((string)elem["tag"]!).StartsWith(tag));
        }
        else
        {
            query = _configuration["outbounds"]!.Where(elem => (string?)elem["tag"] == tag);
        }
        
        query.ToList().ForEach(elem => elem.Remove());
    }

    public void StartProcess()
    {
        if (_process.IsRunning) return;
        _process.Start();
    }

    public async Task KillProcessAsync()
    {
        await _process.KillAsync();
    }

    public void RegisterProcess(IProcessManager processManager)
    {
        processManager.Register(_process);
    }

    private void OnProcessExited(object? sender, EventArgs e)
    {
        ProcessExited?.Invoke(this, e);
    }

    private void OnProcessStarted(object? sender, EventArgs e)
    {
        ProcessStarted?.Invoke(this, e);
    }
}