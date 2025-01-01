using CShroud.Core.Domain.Interfaces;

namespace CShroud.Core.Domain.Services;

public class ProtocolHandlerFactory : IProtocolHandlerFactory
{
    private Dictionary<string, Func<IProtocolHandler>> _handlers = new Dictionary<string, Func<IProtocolHandler>>()
    {
        { "vless",  }
    };
    public Func<IProtocolHandler>? Analyze(string protocolId)
    {
        return _handlers.TryGetValue(protocolId, out var handler) ? handler : null;
    }
}