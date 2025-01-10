namespace CShroudApp.Infrastructure.Interfaces;

public interface IServerRepository
{
    void GetVpnAccess(string token, string protocol);
    bool Login();
    bool Logout();
}