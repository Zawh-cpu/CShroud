using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Data;
namespace CShroud.Infrastructure.Services;

public class BaseRepository : IBaseRepository
{
    public BaseRepository()
    {
        var context = new ApplicationContext();
    }
    public string Ping()
    {
        return "Pong";
    }
}