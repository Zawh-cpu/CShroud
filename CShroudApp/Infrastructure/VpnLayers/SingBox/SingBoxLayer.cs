using System.Diagnostics;
using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Json.Policies;
using CShroudApp.Infrastructure.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public partial class SingBoxLayer : IVpnCoreLayer
{
    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessExited;
    public List<VpnProtocol> SupportedProtocols { get; } = new()
    {
        VpnProtocol.Vless, VpnProtocol.Http, VpnProtocol.Tun
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
    
    public SingBoxLayer(IProcessManager processManager)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.CurrentDirectory, "Binaries", "Cores", "SingBox", "sing-box.exe"),
            Arguments = "-c stdin",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        _process = new BaseProcess(processStartInfo);
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
            Formatting = Formatting.Indented
        };

        await _process.StandardInput.WriteAsync(JsonConvert.SerializeObject(_configuration, settings));
        _process.StandardInput.Close();
        Console.WriteLine("AFAFWQEFWEFWF");
        _process.Start();
    }

    public async Task KillProcessAsync()
    {
        if (!IsRunning) return;
        await _process.KillAsync();
    }

    public void FixDnsIssues(List<string> transparentHosts)
    {
        throw new NotImplementedException();
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