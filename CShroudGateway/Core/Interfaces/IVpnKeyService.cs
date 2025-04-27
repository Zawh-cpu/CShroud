using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IVpnKeyService
{
    Task<Result<Infrastructure.Data.Entities.Key>> AddKey(Guid userId, VpnProtocol protocol, Server server);
    Task<Result> DelKey(Guid userId, Guid keyId);
    Task<Result> DisableKey(Guid userId, Guid keyId);
    Task<Result> EnableKey(Guid userId, Guid keyId);
}