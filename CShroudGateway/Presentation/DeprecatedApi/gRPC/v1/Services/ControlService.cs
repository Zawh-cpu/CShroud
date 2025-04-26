using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;

public class ControlService : Control.ControlBase
{
    private readonly ILogger<ControlService> _logger;
    private readonly IBaseRepository _baseRepository;
    private readonly IVpnService _vpnService;

    public ControlService(ILogger<ControlService> logger, IBaseRepository baseRepository, IVpnService vpnService)
    {
        _logger = logger;
        _baseRepository = baseRepository;
        _vpnService = vpnService;
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
        
        User? user = await _baseRepository.GetUserByIdAsync(userId, x => x.Include(x => x.Rate).Include(x => x.Keys));
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "User with this id doesn't exists"));
        }
        
        if (user.Rate != null && user.Keys.Count >= user.Rate.MaxKeys)
        {
            throw new RpcException(new Status(StatusCode.Cancelled, "Max keys reached"));
        }
        
        Protocol? protocol = await _baseRepository.GetProtocolAsync(dbContext, request.ProtocolId);
        if (protocol == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Protocol with this id doesn't exists"));
        }
        
        var key = new Infrastructure.Data.Entities.Key()
        {
            UserId = user.Id,
            Uuid = Guid.NewGuid().ToString(),
            LocationId = "frankfurt",
            ProtocolId = protocol.Id,
            Name = request.Name.Substring(0, Math.Min(request.Name.Length, 96))
        };

        await dbContext.Keys.AddAsync(key);
        await dbContext.SaveChangesAsync();
        await _vpnRepository.AddKey(user.Rate.VPNLevel, key.Uuid, key.ProtocolId);
        
        return new AddClientResponse()
        {
            Id = key.Id
        };
    }

    
    public override async Task<Empty> DelClient(RemClientRequest request, ServerCallContext context)
    {
        var dbContext = new ApplicationContext();
        
        Infrastructure.Data.Entities.Key? key = await _baseRepository.GetKeyAsync(dbContext, request.KeyId);
        if (key == null)
        {
            return new Empty();
        }

        if (key.UserId != request.UserId)
        {
            return new Empty();
        }

        _ = await _vpnRepository.DelKey(key.Uuid, key.ProtocolId);
        dbContext.Remove(key);
        await dbContext.SaveChangesAsync();
        
        return new Empty();
    }
    
    public override async Task<Empty> EnableKey(KeyRequest request, ServerCallContext context)
    {
        var dbContext = new ApplicationContext();
        
        Infrastructure.Data.Entities.Key? key = await _baseRepository.GetKeyAsync(dbContext, request.KeyId);
        if (key == null || key.UserId != request.UserId) throw new RpcException(new Status(StatusCode.InvalidArgument, "Key with this id doesn't exists"));

        User? user = await _baseRepository.GetUserAsync(dbContext, request.UserId, x => x.Rate!);
        if (user == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "User with this id doesn't exists"));

        int enabledKeys = await _baseRepository.CountKeysAsync(dbContext, user.Id);
        if (enabledKeys >= user.Rate!.MaxKeys) throw new RpcException(new Status(StatusCode.Cancelled, "Max enabled keys reached"));

        if (!key.IsActive)
        {
            
            if (!await _keyService.EnableKey(user, key))
            {
                throw new RpcException((new Status(StatusCode.Aborted, "Unknown error occured.")));
            }
            
            await dbContext.SaveChangesAsync();
        }
                
        return new Empty();
    }
    
    
    public override async Task<Empty> DisableKey(KeyRequest request, ServerCallContext context)
    {
        var dbContext = new ApplicationContext();
        
        Infrastructure.Data.Entities.Key? key = await _baseRepository.GetKeyAsync(dbContext, request.KeyId);
        if (key == null || key.UserId != request.UserId) throw new RpcException(new Status(StatusCode.InvalidArgument, "Key with this id doesn't exists"));

        User? user = await _baseRepository.GetUserAsync(dbContext, request.UserId, x => x.Rate!);
        if (user == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "User with this id doesn't exists"));

        if (key.IsActive)
        {
            if (!await _keyService.DisableKey(key))
            {
                throw new RpcException((new Status(StatusCode.Aborted, "Unknown error occured.")));
            }
            
            await dbContext.SaveChangesAsync();
        }
                
        return new Empty();
    }
}
