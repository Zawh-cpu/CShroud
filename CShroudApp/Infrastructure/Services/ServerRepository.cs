using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public struct Token
{
    public string Data;
    public DateTime Expires;
}

public class ServerRepository: IServerRepository
{
    private Token? _authToken = null;
    private Token? _refreshToken = null;

    public bool MakeAVpnConnection(string token, string protocol, out ConnectionAnswer answer)
    {
        if (_authToken == null || _refreshToken == null)
        {
            answer = new ConnectionAnswer();
            return false;
        };
        
        answer = new ConnectionAnswer()
        {
            
        };
        return true;
    }
    
    public bool Login()
    {
        _authToken = new Token()
        {
            Data = "token",
            Expires = DateTime.UtcNow.AddMinutes(5),
        };

        _refreshToken = new Token()
        {
            Data = "refresh_token",
            Expires = DateTime.UtcNow.AddDays(3)
        };
        
        return true;
    }

    public bool Logout()
    {
        // Retrieve the token
        // Forget the token
        _authToken = null;
        _refreshToken = null;
        return true;
    }
}