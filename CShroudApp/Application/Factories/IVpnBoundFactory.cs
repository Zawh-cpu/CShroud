using CShroudApp.Core.Entities.Vpn.Bounds;
using CShroudApp.Core.Shared.Dto;

namespace CShroudApp.Application.Factories;

public static class IVpnBoundFactory
{
     public static Dictionary<IVpnBound, >
     public static Vless ToDomain(VlessDto dto) => new Vless()
     {
          Tag = dto.Tag,
          Host = dto.Host,
          Port = dto.Port,
          Sniff = dto.Sniff ?? false,
          SniffOverrideDestination = dto.SniffOverrideDestination ?? false
     };
}