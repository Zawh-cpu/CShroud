using CShroud.Core.Domain.Interfaces;

namespace CShroud.Core.Domain.Handlers;

public class VlessHandler : IProtocolHandler
{
    public string MakeAccount()
    {
        return "Vless";
    }
}