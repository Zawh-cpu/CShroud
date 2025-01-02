using System.Linq.Expressions;
using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Interfaces;

public interface IBaseRepository
{
    // Vitas DeMarsh
    string Ping();

    Task<bool> UserExistsAsync(ulong telegramId);
    Task AddUserAsync(User user);
    Task<User?> GetUserAsync(uint id, params Expression<Func<User, object>>[] includes);
    Task<Protocol?> GetProtocolAsync(string id);
    Task AddKeyAsync(Key key);
    Task<Key?> GetKeyAsync(uint id);
    Task DelKeyAsync(Key key);
    Task<int> CountKeysAsync(uint userId, bool active = true);

    Task SaveAsync();
}