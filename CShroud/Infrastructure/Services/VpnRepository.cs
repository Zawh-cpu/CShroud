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
    
    public async Task<TResponse?> MakeRequest<TRequest, TResponse>(Func<TRequest, AsyncUnaryCall<TResponse>> grpcMethod, TRequest request)
    {
        try
        {
            var call = grpcMethod(request);
        
            // Дождаться ответа
            var response = await call.ResponseAsync;
        
            // Получить статус
            var status = call.GetStatus();

            if (status.StatusCode != Grpc.Core.StatusCode.OK)
            {
                Console.WriteLine($"gRPC error: {status.StatusCode} - {status.Detail}");
                return default; // Возвращаем null, если ошибка
            }

            return response; // Возвращаем успешный ответ
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC RpcException: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }

        return default; // Возвращаем null в случае ошибки
    }


    
    public async Task AddUser(Xray.Common.Protocol.User user, Protocol protocol)
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

        AlterInboundResponse result = await MakeRequest(client.AlterInboundAsync, request);
        // Console.WriteLine(result.GetStatus().StatusCode.GetType());
    }
}