using CShroud.Core.Domain.Interfaces;
using CShroud.Core.Domain.Services;
using CShroud.Infrastructure.Data.Entities;
using CShroud.Infrastructure.Interfaces;
using CShroud.Presentation.Protos.Server;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;

namespace CShroud.Presentation.gRPC.v1;

using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xray.App.Proxyman.Command;

public class ControlService : Control.ControlBase
{
    private readonly ILogger<ControlService> _logger;
    private readonly GrpcChannel _channel;
    private readonly Infrastructure.Services.Core _core;
    private readonly IBaseRepository _baseRepository;
    private readonly IProtocolHandlerFactory _protocolHandlerFactory;

    public ControlService(ILogger<ControlService> logger, GrpcChannel channel, Infrastructure.Services.Core core, IBaseRepository baseRepository, IProtocolHandlerFactory protocolHandlerFactory)
    {
        _logger = logger;
        _channel = channel;
        _core = core;
        _baseRepository = baseRepository;
        _protocolHandlerFactory = protocolHandlerFactory;
    }

    public override async Task<Empty> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        if (await _baseRepository.UserExistsAsync(request.TelegramId))
        {
            throw new RpcException(new Status(StatusCode.Cancelled, "User already exists"));
        }

        var user = new User()
        {
            Nickname = request.Nickname.Substring(0, Math.Min(request.Nickname.Length, 96)),
            TelegramId = request.TelegramId
        };
        
        await _baseRepository.AddUserAsync(user);

        return await Task.FromResult(new Empty());
    }
    
    public override async Task<AddClientResponse> AddClient(AddClientRequest request, ServerCallContext context)
    {
        User? user = await _baseRepository.GetUserAsync(request.UserId, x => x.Keys, x => x.Rate);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "User with this id doesn't exists"));
        }
        
        if (user.Keys.Count >= user.Rate!.MaxKeys)
        {
            throw new RpcException(new Status(StatusCode.Cancelled, "Max keys reached"));
        }
        
        Protocol? protocol = await _baseRepository.GetProtocolAsync(request.ProtocolId);
        if (protocol == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Protocol with this id doesn't exists"));
        }
        
        if (!_protocolHandlerFactory.Analyze(request.ProtocolId, out var protocolToolsFactory))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Protocol with this id doesn't exists"));
        }

        IProtocolHandler protocolTools = protocolToolsFactory!.Invoke();
        
        var client = new HandlerService.HandlerServiceClient(_channel);

        var parsedArgs = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Args);
        
        
        var uuid = Guid.NewGuid().ToString();
        
        var userCmd = new AddUserOperation()
        {
            User = new Xray.Common.Protocol.User()
            {
                Level = user.Rate.VPNLevel,
                Email = $"{user.Id}_{uuid}",
                Account = protocolTools.MakeAccount(uuid, parsedArgs)
            }
        };
        
        var key = new CShroud.Infrastructure.Data.Entities.Key()
        {
            UserId = user.Id,
            Uuid = uuid,
            LocationId = "frankfurt",
            ProtocolId = protocol.Id,
            Port = protocol.Port,
            Name = request.Name.Substring(0, Math.Min(request.Name.Length, 96))
        };

        await _baseRepository.AddKeyAsync(key);
        
        return await Task.FromResult(new AddClientResponse()
        {
            Id = (uint)key.Id
        });
    }

    /*
    public override Task<Empty> DelClient(RemClientRequest request, ServerCallContext context)
    {
        ApplicationContext session = new ApplicationContext();
        Key? key = session.Keys.Where(key => key.Id == request.KeyId && key.UserId == request.UserId).SingleOrDefault();
        if (key != null)
        {
            
            var client = new HandlerService.HandlerServiceClient(_channel);
            _ = client.AlterInbound(new AlterInboundRequest()
            {
                Tag = $"inbound-{key.ProtocolId}",
                Operation = Core.ToTypedMessage(new RemoveUserOperation()
                {
                    Email = $"{key.UserId}_{key.Uuid}"
                })
            });
            
            session.Keys.Remove(key);
            session.SaveChanges();
        }
        
        return Task.FromResult(new Empty());
    }
    
    public override Task<Empty> EnableKey(KeyRequest request, ServerCallContext context)
    {
        ApplicationContext session = new ApplicationContext();
        Key? key = session.Keys.Where(key => key.Id == request.KeyId && key.UserId == request.UserId).SingleOrDefault();
        if (key == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Key with this id doesn't exists"));
        
        User? user = session.Users.Include(x => x.Rate).Where(user => user.Id == request.UserId).SingleOrDefault();
        if (user == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "User with this id doesn't exists"));

        int enabledKeys = session.Keys.AsNoTracking().Where(k => k.UserId == user.Id && k.IsActive).Count();
        if (enabledKeys >= user.Rate!.MaxKeys) throw new RpcException(new Status(StatusCode.Cancelled, "Max enabled keys reached"));

        if (!key.IsActive)
        {
            if (Utils.Utils.EnableKey(_core, user, key))
            {
                key.IsActive = true;
                session.SaveChanges();
                return Task.FromResult(new Empty());
            }
            
            throw new RpcException((new Status(StatusCode.Aborted, "Unknown error occured.")));
        }
                
        return Task.FromResult(new Empty());
    }
    
    public override Task<Empty> DisableKey(KeyRequest request, ServerCallContext context)
    {
        ApplicationContext session = new ApplicationContext();
        Key? key = session.Keys.Where(key => key.Id == request.KeyId && key.UserId == request.UserId).SingleOrDefault();
        if (key == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Key with this id doesn't exists"));
        
        User? user = session.Users.Include(x => x.Rate).Where(user => user.Id == request.UserId).SingleOrDefault();
        if (user == null) throw new RpcException(new Status(StatusCode.InvalidArgument, "User with this id doesn't exists"));

        if (key.IsActive)
        {
            if (Utils.Utils.DisableKey(_core.XrayCoreChannel, key))
            {
                key.IsActive = false;
                session.SaveChanges();
                return Task.FromResult(new Empty());
            }
            
            throw new RpcException((new Status(StatusCode.Aborted, "Unknown error occured.")));
        }
                
        return Task.FromResult(new Empty());
    }*/
}
