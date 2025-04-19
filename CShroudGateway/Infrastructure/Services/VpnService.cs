namespace CShroudGateway.Infrastructure.Services;

public class VpnService
{
    public Dictionary<Guid, string> Connections { get; private set; } = new();
    // { UUID: [ServerUUID, ServerUUID, ...] }

}