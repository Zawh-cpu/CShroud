using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Interfaces;

public interface IKeyService
{
    Task<bool> EnableKey(User user, Key key, bool save = true);
    Task<bool> DisableKey(Key key, bool save = true);
}