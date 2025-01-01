using Google.Protobuf;
using Xray.Common.Serial;

namespace CShroud.Infrastructure.Interfaces;

public interface IVpnRepository
{
    public static TypedMessage ToTypedMessage(IMessage message)
    {
        ByteString serializedMessage = message.ToByteString();
    
        return new TypedMessage
        {
            Type = message.Descriptor.FullName,
            Value = serializedMessage
        };
    }
}