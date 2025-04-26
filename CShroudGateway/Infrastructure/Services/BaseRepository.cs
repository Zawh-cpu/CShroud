using System.Linq.Expressions;
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
    
    public async Task<int> CountKeysAsync(Guid userId)
    {
        return await _context.Keys.Where(key => key.UserId == userId).CountAsync();
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

    public async Task<bool> IsUserWithThisTelegramIdExistsAsync(ulong telegramId)
    {
        return await _context.Users.Where(user => user.TelegramId == telegramId).AnyAsync();
    }
}