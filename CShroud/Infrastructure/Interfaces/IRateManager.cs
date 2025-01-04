using CShroud.Infrastructure.Data.Entities;

namespace CShroud.Infrastructure.Interfaces;

public interface IRateManager
{
    Task UpdateRate(User user);
}