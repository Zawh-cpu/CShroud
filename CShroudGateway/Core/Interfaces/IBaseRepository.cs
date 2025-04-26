using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Core.Interfaces;

public interface IBaseRepository
{
    Task<bool> IsUserWithThisTelegramIdExistsAsync(ulong telegramId);
    Task<int> CountKeysAsync(Guid userId);
    Task<User?> GetUserByIdAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers);
    Task AddWithSaveAsync<TEntity>(TEntity entity) where TEntity : class;
}