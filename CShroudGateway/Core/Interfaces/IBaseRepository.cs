using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public record UserWithKeys(User User, int KeysCount);

public interface IBaseRepository
{
    Task<bool> IsUserWithThisTelegramIdExistsAsync(ulong telegramId);
    Task<int> CountKeysAsync(Guid userId);
    Task<User?> GetUserByIdAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    Task AddWithSaveAsync<TEntity>(TEntity entity) where TEntity : class;
    Task DelWithSaveAsync<TEntity>(TEntity entity) where TEntity : class;

    Task<UserWithKeys?> GetUserByIdWithKeyCountAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);

    Task<Key?> GetKeyByIdAsync(Guid keyId, params Func<IQueryable<Key>, IQueryable<Key>>[] queryModifiers);
    Task<List<Server>?> GetServersByLocationAndProtocolsAsync(string location, HashSet<VpnProtocol> protocols, uint limit = 3, params Func<IQueryable<Server>, IQueryable<Server>>[] queryModifiers);

    Task SaveContextAsync();
}