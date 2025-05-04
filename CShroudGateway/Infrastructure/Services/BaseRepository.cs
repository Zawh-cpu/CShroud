using System.Linq.Expressions;
using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Infrastructure.Services;

public class BaseRepository : IBaseRepository
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CountKeysAsync(Guid userId, Expression<Func<Key, bool>>? predicate)
    {
        var query = _context.Keys.Where(key => key.UserId == userId);
        if (predicate is not null)
            query = query.Where(predicate);
        
        return await query.CountAsync();
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers)
    {
        IQueryable<User> query = _context.Users.Where(user => user.Id == userId);
        
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task AddWithSaveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync<TEntity>(TEntity entity, bool saveChanges = true) where TEntity : class
    {
        await _context.Set<TEntity>().AddRangeAsync(entity);
        if (saveChanges) await _context.SaveChangesAsync();
    }

    public async Task<Rate?> GetFirstDefaultRateAsync()
    {
        return await _context.Rates.OrderBy(r => r.Id).FirstOrDefaultAsync();
    }
    
    public async Task DelWithSaveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserWithThisTelegramIdExistsAsync(ulong telegramId)
    {
        return await _context.Users.Where(user => user.TelegramId == telegramId).AnyAsync();
    }

    public async Task<UserWithKeys?> GetUserByIdWithKeyCountAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers)
    {
        var query = _context.Users.Where(user => user.Id == userId);
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }
        
        return await query.Select(u => new UserWithKeys(u, u.Keys.Count)).FirstOrDefaultAsync();
    }

    public async Task<UserKeyActiveKeysCount?> GetUserKeysActiveKeysCountByIdsAsync(Guid userId, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers)
    {
        var query = _context.Users.Where(user => user.Id == userId);
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }
        
        return await query.Select(u => new UserKeyActiveKeysCount(u, u.Keys.Count, u.Keys.Count(k => k.Status == KeyStatus.Enabled))).FirstOrDefaultAsync();
    }

    public Task<User[]> GetUsersPayedUntilAsync(Expression<Func<User, bool>> predicate, params Func<IQueryable<User>, IQueryable<User>>[] queryModifiers)
    {
        var query = _context.Users.Where(predicate);
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }

        return query.ToArrayAsync();
    }

    public async Task<Key?> GetKeyByIdAsync(Guid keyId, params Func<IQueryable<Key>, IQueryable<Key>>[] queryModifiers)
    {
        var query = _context.Keys.Where(key => key.Id == keyId);
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Server>?> GetServersByLocationAndProtocolsAsync(string location, HashSet<VpnProtocol> protocols,
        int limit = 3, params Func<IQueryable<Server>, IQueryable<Server>>[] queryModifiers)
    {
        
        var query = _context.Servers.Where(server => server.Location == location && server.SupportedProtocols.Any(p => protocols.Contains(p)));
        foreach (var modifier in queryModifiers)
        {
            query = modifier(query);
        }
        
        return await query.OrderByDescending(s => s.Id).Take(limit).ToListAsync();
    }
    
    public async Task SaveContextAsync() => await _context.SaveChangesAsync();
}