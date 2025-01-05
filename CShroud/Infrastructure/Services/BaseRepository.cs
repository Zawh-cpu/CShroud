using System.Linq.Expressions;
using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroud.Infrastructure.Services;

public class BaseRepository : IBaseRepository
{
    private readonly ApplicationContext _context;
    
    public BaseRepository()
    {
        _context = new ApplicationContext();
    }
    public string Ping()
    {
        return "Pong";
    }
    
    public async Task<bool> UserExistsAsync(ulong telegramId)
    {
        return await _context.Users.AnyAsync(u => u.TelegramId == telegramId);
    }   

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    private static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        if (propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member.Name;
        }

        throw new InvalidOperationException("Invalid navigation property expression.");
    }
    
    public async Task ExplicitLoadAsync<T>(
        T entity,
        params Expression<Func<T, object>>[] navigationProperties
    ) where T : class
    {
        foreach (var navigationProperty in navigationProperties)
        {
            string propertyName = GetPropertyName(navigationProperty);

            var entry = _context.Entry(entity);
            
            var isCollection = entry.Metadata.FindNavigation(propertyName)?.IsCollection ?? false;

            if (isCollection)
            {
                await entry.Collection(propertyName).LoadAsync();
            }
            else
            {
                await entry.Reference(propertyName).LoadAsync();
            }
        }
    }
    
    public async Task<User?> GetUserAsync(uint id, params Expression<Func<User, object>>[] includes)
    {
        IQueryable<User> query = _context.Users;

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await query.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Protocol?> GetProtocolAsync(string id)
    {
        return await _context.Protocols.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddKeyAsync(Key key)
    {
        await _context.Keys.AddAsync(key);
        await _context.SaveChangesAsync();
    }

    public async Task<Key?> GetKeyAsync(uint id)
    {
        return await _context.Keys.FirstOrDefaultAsync(k => k.Id == id);
    }
    
    public async Task DelKeyAsync(Key key)
    {
        _context.Keys.Remove(key);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountKeysAsync(uint userId, bool active = true)
    {
        return await _context.Keys.AsNoTracking().Where(k => k.UserId == userId && k.IsActive == active).CountAsync();
    }

    public async Task<List<User>> GetAllActiveKeysByUserAsync()
    {
        return await _context.Users.Where(u => u.RateId > 1).Include(u => u.Rate).Include(u => u.Keys.Where(k => k.IsActive == true)).ToListAsync();
    }
    
    public async Task<List<User>> GetUsersPayedUntilAsync(DateTime date)
    {
        return await _context.Users.Where(u => u.PayedUntil <= date).ToListAsync();
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}