namespace CShroudGateway.Core.Interfaces;

public interface IVpnService
{
    Dictionary<Guid, List<string>> Connections { get; set; }

    Task AddKey();
    Task DelKey();
    Task DisableKey();
    Task EnableKey();
}