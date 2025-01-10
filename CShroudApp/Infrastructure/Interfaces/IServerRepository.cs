using CShroudApp.Core.Domain.Entities;

namespace CShroudApp.Infrastructure.Interfaces;

public interface IServerRepository
{
    ConnectionAnswer GetVpnAccess(string token, string protocol);
    bool Login();
    bool Logout();
}