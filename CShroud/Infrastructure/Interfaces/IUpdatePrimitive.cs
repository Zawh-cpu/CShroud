using CShroud.Presentation.Protos.Server;

namespace CShroud.Infrastructure.Interfaces;

public interface IUpdatePrimitive
{
    string GlobalParamsHash { get; }
    UpdateBytes ProtoGlobalParams { get; }
}