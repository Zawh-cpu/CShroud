using CShroudApp.Core.Domain.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IServerRepository
{
    bool MakeAVpnConnection(string location, string protocol, out ConnectionAnswer answer);
    bool Login();
    bool Logout();
}