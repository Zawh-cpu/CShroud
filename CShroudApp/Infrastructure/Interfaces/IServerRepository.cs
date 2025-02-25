using CShroudApp.Core.Domain.Entities;

namespace CShroudApp.Infrastructure.Interfaces;

public interface IServerRepository
{
    bool MakeAVpnConnection(string location, string protocol, out ConnectionAnswer answer);
    bool Login();
    bool Logout();
}