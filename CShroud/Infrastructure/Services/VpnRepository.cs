using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Data.Entities;
using CShroud.Infrastructure.Interfaces;
using Grpc.Core;
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
    
    private async Task<TResponse?> MakeRequest<TRequest, TResponse>(Func<TRequest, CallOptions, AsyncUnaryCall<TResponse>> grpcMethod, TRequest request)
    {
        try
        {
            var call = grpcMethod(request, new CallOptions());
            var response = await call.ResponseAsync;
            var status = call.GetStatus();

            if (status.StatusCode != Grpc.Core.StatusCode.OK)
            {
                Console.WriteLine($"gRPC error: {status.StatusCode} - {status.Detail}");
                return default;
            }

            return response;
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC RpcException: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }

        return default;
    }


    
    public async Task<bool> AddUser(Xray.Common.Protocol.User user, Protocol protocol)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);
        
        var userCmd = new AddUserOperation()
        {
            User = user
        };
        
        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{protocol.Id}",
            Operation = IVpnRepository.ToTypedMessage(userCmd)
        };

        var result = await MakeRequest(client.AlterInboundAsync, request);
        if (result != null) return true;
        return false;
    }

    public async Task<bool> DelUser(Key key)
    {
        var client = new HandlerService.HandlerServiceClient(_channel);

        var request = new AlterInboundRequest()
        {
            Tag = $"inbound-{key.ProtocolId}",
            Operation = IVpnRepository.ToTypedMessage(new RemoveUserOperation()
            {
                Email = $"{key.UserId}_{key.Uuid}"
            })
        };
        
        var result = await MakeRequest(client.AlterInboundAsync, request);
        if (result != null) return true;
        return false;
    }
}