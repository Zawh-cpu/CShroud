using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Data.Entities;
using CShroud.Infrastructure.Interfaces;
using Grpc.Net.Client;
using Xray.App.Proxyman.Command;

namespace CShroud.Infrastructure.Services;

public class VpnRepository : IVpnRepository
{
    private GrpcChannel _channel;
    private readonly IVpnCore _vpnCore;
    private readonly VpnCoreConfig _vpnCoreConfig;
    
    public VpnRepository(IVpnCore vpnCore, VpnCoreConfig vpnCoreConfig)
    {
        _vpnCore = vpnCore;
        _vpnCoreConfig = vpnCoreConfig;
        
        _channel = GrpcChannel.ForAddress(_vpnCoreConfig.Link);
    }
    
    
    public void AddUser(Xray.Common.Protocol.User user, Protocol protocol)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);
        
        var userCmd = new AddUserOperation()
        {
            User = user
        };
        
        var result = client.AlterInbound(new AlterInboundRequest()
        {
            Tag = $"inbound-{protocol.Id}",
            Operation = IVpnRepository.ToTypedMessage(userCmd)
        });
        
        Console.WriteLine(result);
    }
}