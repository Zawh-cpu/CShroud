using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using IBaseRepository = CShroud.Infrastructure.Interfaces.IBaseRepository;

namespace CShroudGateway.Infrastructure.Services;

public class VpnServerManager : IVpnServerManager
{
    private readonly IBaseRepository _baseRepository;

    public VpnServerManager(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }
    
    public Task<Server> GetAvailableServerAsync(string location, VpnProtocol protocol)
    {
        var server = await _baseRepository.GetServerByLocationAndProtocolsAsync(location, [protocol]);
        throw new NotImplementedException();
    }
}