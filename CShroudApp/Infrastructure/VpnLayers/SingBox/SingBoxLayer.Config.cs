using CShroudApp.Core.Entities.Vpn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers.SingBox;

public class SingBoxConfig
{
    public LogObject Log { get; set; } = new();
    public DnsObject Dns { get; set; } = new();
    public Dictionary<string, object> Ntp { get; set; } = new();
    public Dictionary<string, object> Certificate { get; set; } = new();
    public Dictionary<string, object> Endpoints { get; set; } = new();
    public List<BoundObject> Inbounds { get; set; } = new();
    public List<BoundObject> Outbounds { get; set; } = new();
    public Dictionary<string, object> Route { get; set; } = new();
    public Dictionary<string, object> Experimental { get; set; } = new();

    public class LogObject
    {
        public bool Disabled { get; set; } = false;
        public string Level { get; set; } = "info";
        public string Output { get; set; } = "box.log";
        public bool Timestamp { get; set; } = true;
    }

    public class DnsObject
    {
        public List<object> Servers { get; set; } = new();
        public List<object> Rules { get; set; } = new();
        public string Final { get; set; } = "";
        public string Strategy  { get; set; } = "";
        public bool DisableCache { get; set; } = false;
        public bool DisableExpire { get; set; } = false;
        public bool IndependentCache { get; set; } = false;
        public int CacheCapacity { get; set; } = 0;
        public bool ReverseMapping { get; set; } = false;
        public string ClientSubnet { get; set; } = "";
        public Dictionary<string, object> Fakeip { get; set; } = new();
    }

    public class BoundObject
    {
        public required string Type { get; set; }
        public required string Tag { get; set; }
        
        [JsonExtensionData]
        public JObject Extra { get; set; } = new();
        
    }
}