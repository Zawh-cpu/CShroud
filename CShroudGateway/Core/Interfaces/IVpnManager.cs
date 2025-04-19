namespace CShroudGateway.Core.Interfaces;

public interface IVpService
{
    Dictionary<Guid, List<Guid>> Connections { get; }
    
    public Task DisconnectAsync(Guid connectionId);
    public Task ConnectAsync(Guid connectionId, Guid serverId);
}