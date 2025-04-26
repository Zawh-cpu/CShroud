using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Services;

public class VpnService : IVpnService
{
    public Dictionary<Guid, List<string>> Connections { get; set; } = new();

    public Task AddKey()
    {
        throw new NotImplementedException();
    }

    public Task DelKey()
    {
        throw new NotImplementedException();
    }

    public Task DisableKey()
    {
        throw new NotImplementedException();
    }

    public Task EnableKey()
    {
        throw new NotImplementedException();
    }
    // { UUID: [ServerUUID, ServerUUID, ...] }
}