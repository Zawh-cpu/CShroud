using CShroud.Infrastructure.Interfaces;
using System.Collections.Generic;
using CShroud.Core.Domain.Interfaces;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Services;

public class KeyService : IKeyService
{
    private readonly IVpnRepository _vpnRepository;
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnCore _vpnCore;
    private HashSet<string> _activeKeys = new();
    
    public KeyService(IVpnRepository vpnRepository, IBaseRepository baseRepository, IVpnCore vpnCore)
    {
        _vpnRepository = vpnRepository;
        _baseRepository = baseRepository;
        _vpnCore = vpnCore;

        _vpnCore.VpnStarted += (object? sender, EventArgs e) => Task.Run(async() => await LoadActiveKeysOnStart(sender, e));
    }

    private async Task LoadActiveKeysOnStart(object? sender, EventArgs e)
    {
        var users = await _baseRepository.GetAllActiveKeysByUserAsync();
        foreach (var user in users)
        {
            foreach (var key in user.Keys)
            {
                await _vpnRepository.AddKey(user.Rate!.VPNLevel, key.Uuid, key.ProtocolId);
            }
        }
    }
    
    public async Task<bool> EnableKey(User user, Key key, bool save = true)
    {
        if (!await _vpnRepository.AddKey(user.Rate!.VPNLevel, key.Uuid, key.ProtocolId)) return false;
        key.IsActive = true;
        if (save) await _baseRepository.SaveAsync();
        
        return true;
    }

    public async Task<bool> DisableKey(Key key, bool save = true)
    {
        if (!await _vpnRepository.DelKey(key.Uuid, key.ProtocolId)) return false;
        key.IsActive = false;
        
        if (save) await _baseRepository.SaveAsync();
        
        return true;
    }
}