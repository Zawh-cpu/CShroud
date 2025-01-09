namespace CShroudApp.Infrastructure.Interfaces;

public interface IServerRepository
{
    void ConnectToVpn(string token, string protocol);
    bool Login();
    bool Logout();
}