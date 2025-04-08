using CShroudApp.Infrastructure.Data.Config;
using Newtonsoft.Json.Linq;

namespace CShroudApp.Infrastructure.VpnLayers;

public partial class SingBoxLayer
{
    private void ApplyConfigToCoreConfiguration()
    {
        var servers = _configuration.Value<JObject>("dns")!.Value<JArray>("servers")!
            .Where(x => ((JObject)x).Value<string>("tag")!.EndsWith("_dns")).ToList();

        var remoteDns = servers.First(x => x.Value<string>("tag") == "remote_dns");
        remoteDns["address"] = _settingsConfig.Network.Dns.ForVpnServer;
        
        var localDns = servers.First(x => x.Value<string>("tag") == "local_dns");
        localDns["address"] = _settingsConfig.Network.Dns.ForLocalServer;
    }
}