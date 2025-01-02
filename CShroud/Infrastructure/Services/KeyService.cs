using CShroud.Infrastructure.Interfaces;
using System.Collections.Generic;
using CShroud.Core.Domain.Interfaces;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Services;

public class KeyService : IKeyService
{
    private readonly IVpnRepository _vpnRepository;
    private readonly IBaseRepository _baseRepository;
    private readonly IProtocolHandlerFactory _protocolHandlerFactory;
    private HashSet<string> _activeKeys = new();
    
    public KeyService(IVpnRepository vpnRepository, IBaseRepository baseRepository, IProtocolHandlerFactory protocolHandlerFactory)
    {
        _vpnRepository = vpnRepository;
        _baseRepository = baseRepository;
        _protocolHandlerFactory = protocolHandlerFactory;
    }

    public async Task<bool> EnableKey(User user, Key key)
    {
        if (!await _vpnRepository.AddKey(user.Rate!.VPNLevel, key.Uuid, key.ProtocolId)) return false;
        key.IsActive = true;
        await _baseRepository.SaveAsync();
        
        return true;
    }

    public async Task<bool> DisableKey(Key key)
    {
        if (!await _vpnRepository.DelKey(key.Uuid, key.ProtocolId)) return false;
        key.IsActive = false;
        await _baseRepository.SaveAsync();
        
        return true;
    }
}