using Ardalis.Result;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Key = Microsoft.EntityFrameworkCore.Metadata.Internal.Key;

namespace CShroudGateway.Infrastructure.Services;

public class VpnKeyService : IVpnKeyService
{
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnRepository _vpnRepository;
    
    public VpnKeyService(IBaseRepository baseRepository, IVpnRepository vpnRepository)
    {
        _baseRepository = baseRepository;
        _vpnRepository = vpnRepository;
    }
    
    public async Task<Result<Data.Entities.Key>> AddKey(Guid userId, VpnProtocol protocol, Server server)
    {
        var projection = await _baseRepository.GetUserByIdWithKeyCountAsync(userId, x => x.Include(u => u.Rate));
        if (projection is null) return Result.Unauthorized();

        if (projection.User.Rate?.MaxKeys is null || projection.KeysCount >= projection.User.Rate.MaxKeys) return Result.Forbidden();

        var key = new Data.Entities.Key()
        {
            UserId = userId,
            ServerId = server.Id,
            Protocol = protocol,
        };

        var result = await _vpnRepository.AddKey(server, key.Id, key.Protocol, projection.User.Rate.VpnLevel, "{}");
        if (!result.IsSuccess) return Result.Error();

        await _baseRepository.AddWithSaveAsync(key);

        return key;
    }

    public async Task<Result> DelKey(Guid userId, Guid keyId)
    {
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key is null || key.UserId != userId) return Result.Unauthorized();
        
        if (key.Server is not null)
            await _vpnRepository.DelKey(key.Server, key.Id, key.Protocol);
        
        await _baseRepository.DelWithSaveAsync(key);
        return Result.Success();
    }

    public async Task<Result> EnableKey(Guid userId, Guid keyId)
    {
        var projection = await _baseRepository.GetUserKeysActiveKeysCountByIdsAsync(userId, x => x.Include(u => u.Rate));
        if (projection?.User.Rate is null) return Result.Unauthorized();
        if (projection.ActiveKeysCount >= projection.User.Rate.MaxKeys) return Result.Forbidden();
        
        var key = await _baseRepository.GetKeyByIdAsync(userId, x => x.Include(k => k.Server));
        if (key?.Server is null || key.UserId != userId) return Result.Unauthorized();
        
        var result = await _vpnRepository.AddKey(key.Server, key.Id, key.Protocol, projection.User.Rate.VpnLevel, "{}");
        if (!result.IsSuccess) return Result.Error();
        
        key.IsActive = true;
        await _baseRepository.SaveContextAsync();
        return Result.Success();
    }

    public async Task<Result> DisableKey(Guid userId, Guid keyId)
    {
        var key = await _baseRepository.GetKeyByIdAsync(keyId, x => x.Include(k => k.Server));
        if (key is null || key.UserId != userId) return Result.Unauthorized();
        if (key.IsRevoked || key.Server is null) return Result.Forbidden();
        if (!key.IsActive) return Result.Success();
        
        await _vpnRepository.DelKey(key.Server, key.Id, key.Protocol);
        
        key.IsActive = false;
        await _baseRepository.SaveContextAsync();
        return Result.Success();
    }
}