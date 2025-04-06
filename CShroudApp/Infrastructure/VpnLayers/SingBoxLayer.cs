using CShroudApp.Core.Entities.Vpn;
using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer : IVpnCoreLayer
{
    private readonly List<VpnProtocol> _vpnSupportedProtocols = [VpnProtocol.Vless, VpnProtocol.Socks, VpnProtocol.Http];
    private readonly Dictionary<VpnProtocol, Func<IVpnBound, JObject>> _vpnProtocolsHandlers = new()
    {
        { VpnProtocol.Vless, inbound => ParseVlessBound((Vless)inbound) },
        { VpnProtocol.Http, inbound => ParseHttpBound((Http)inbound) },
        { VpnProtocol.Socks, inbound => ParseSocksBound((Socks)inbound) },
    };
    
    private JObject _configuration;

    private readonly string _configurationPath =
        Path.Combine(Environment.CurrentDirectory, "Binaries", "Cores", "SingBox", "config.json");

    private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented
    };

    public SingBoxLayer()
    {
        _configuration = JObject.Parse(File.ReadAllText(_configurationPath));

        NormalizeConfiguration();
        SaveConfiguration();
    }

    public void SaveConfiguration()
    {
        File.WriteAllText(_configurationPath, JsonConvert.SerializeObject(_configuration, _serializerSettings));
    }

    public bool IsProtocolSupported(VpnProtocol protocol)
    {
        return _vpnSupportedProtocols.Contains(protocol);
    }

    public void AddInbound(IVpnBound bound)
    {
        if (!IsProtocolSupported(bound.Type)) throw new NotSupportedException();
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        ((JArray)_configuration["inbounds"]!).Add(action(bound));
    }

    public void AddOutbound(IVpnBound bound)
    {
        if (!IsProtocolSupported(bound.Type)) throw new NotSupportedException();
        if (!_vpnProtocolsHandlers.TryGetValue(bound.Type, out var action)) throw new NotSupportedException();
        
        ((JArray)_configuration["outbounds"]!).Add(action(bound));
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
}