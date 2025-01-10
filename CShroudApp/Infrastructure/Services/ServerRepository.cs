using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

public class ServerRepository: IServerRepository
{
    private string _authToken = string.Empty;

    public void GetVpnAccess(string token, string protocol)
    {
        var settings = _
    }

    public bool Login()
    {
        _authToken = "Token";
        return true;
    }

    public bool Logout()
    {
        // Retrieve the token
        // Forget the token
        _authToken = string.Empty;
        return true;
    }
}