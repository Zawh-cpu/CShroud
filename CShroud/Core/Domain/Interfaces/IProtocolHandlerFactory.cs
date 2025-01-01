namespace CShroud.Core.Domain.Interfaces;

public interface IProtocolHandlerFactory
{
    Func<IProtocolHandler>? Analyze(string protocolId);
}