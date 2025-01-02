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
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}