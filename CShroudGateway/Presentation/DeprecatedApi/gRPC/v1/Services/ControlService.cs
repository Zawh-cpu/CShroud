using CShroudGateway.Core.Entities;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Enum = System.Enum;

namespace CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;

public class ControlService : Control.ControlBase
{
    private readonly ILogger<ControlService> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnKeyService _vpnKeyService;
    private readonly IVpnService _vpnService;
    private readonly IVpnServerManager _vpnServerManager;

    public ControlService(ILogger<ControlService> logger, IBaseRepository baseRepository, IVpnKeyService vpnKeyService, IVpnService vpnService, IVpnServerManager vpnServerManager)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _vpnKeyService = vpnKeyService;
        _vpnService = vpnService;
        _vpnServerManager = vpnServerManager;
    }

    public override async Task<Empty> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        if (await _baseRepository.IsUserWithThisTelegramIdExistsAsync(request.TelegramId))
            throw new RpcException(new Status(StatusCode.Cancelled, "User already exists"));

        var user = new User()
        {
            Nickname = request.Nickname.Substring(0, Math.Min(request.Nickname.Length, 96)),
            TelegramId = request.TelegramId
        };

        await _baseRepository.AddWithSaveAsync(user);
        
        return new Empty();
    }
    
    public override async Task<AddClientResponse> AddClient(AddClientRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));

        if (!Enum.TryParse(request.ProtocolId, out VpnProtocol protocol))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid protocol specified"));

        var server = await _vpnServerManager.GetAvailableServerAsync("frankfurt", protocol);
        if (server is null) throw new RpcException(new Status(StatusCode.NotFound, "Server not found or its under maintenance"));
        
        var result = await _vpnKeyService.AddKey(userId, protocol, server);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error or invalid arguments. Please, try later"));
        
        return new AddClientResponse()
        {
            Id = result.Value.Id.ToString()
        };
    }
    
    
    
    public override async Task<Empty> DelClient(RemClientRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));
        
        var result = await _vpnKeyService.DelKey(userId, keyId);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
    
    public override async Task<Empty> EnableKey(KeyRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));
        
        var result = await _vpnKeyService.EnableKey(userId, keyId);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
    
    
    public override async Task<Empty> DisableKey(KeyRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out Guid userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId is invalid"));
        
        if (!Guid.TryParse(request.KeyId, out Guid keyId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "KeyId is invalid"));
        
        var result = await _vpnKeyService.DisableKey(userId, keyId);
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, "Internal error, unauthorized access or invalid arguments. Please, try later"));
        
        return new Empty();
    }
}
