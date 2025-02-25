using System.Buffers.Text;
using System.Text;
using System.Text.Json.Serialization;
using CShroudApp.Core.Domain.Entities;
using CShroudApp.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace CShroudApp.Infrastructure.Services;

public struct Token
{
    public string Data;
    public DateTime Expires;
}

public class ServerRepository: IServerRepository
{
    private Token? _authToken = null;
    private Token? _refreshToken = null;

    public bool MakeAVpnConnection(string location, string protocol, out ConnectionAnswer answer)
    {
        if (_authToken == null || _refreshToken == null)
        {
            answer = new ConnectionAnswer();
            return false;
        };

        var data =
            "eyJ0eXBlIjogInZsZXNzIiwidGFnIjogInByb3h5Iiwic2VydmVyIjogImZyYW5rZnVydC5yZWFsaXR5Lnphd2gucnUiLCJzZXJ2ZXJfcG9ydCI6IDQ0MywidXVpZCI6ICI4ZDUwZGE0ZS1mZmY0LTQxODgtYmJkMS03ZDYyMGM3Mjk2ZjAiLCJmbG93IjogInh0bHMtcnByeC12aXNpb24iLCJwYWNrZXRfZW5jb2RpbmciOiAieHVkcCIsInRscyI6IHsiZW5hYmxlZCI6IHRydWUsInNlcnZlcl9uYW1lIjogImdvb2dsZS5jb20iLCJpbnNlY3VyZSI6IGZhbHNlLCJ1dGxzIjogeyJlbmFibGVkIjogdHJ1ZSwiZmluZ2VycHJpbnQiOiAicmFuZG9tIn0sInJlYWxpdHkiOiB7ImVuYWJsZWQiOiB0cnVlLCJwdWJsaWNfa2V5IjogIjhBWlFsamJTanZQTVB2Y2ppelBNNEpwVG1jSEJQV3hfc3RNX2gwZ29mRUkiLCJzaG9ydF9pZCI6ICI0YWU2MGI2NGI1Y2QifX19";
        
        answer = new ConnectionAnswer()
        {
            
            Protocol = "vless",
            Location = "frankfurt",
            Params = {},
            Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(Encoding.UTF8.GetString(Convert.FromBase64String(data)))!,
        };
        
        return true;
    }
    
    public bool Login()
    {
        _authToken = new Token()
        {
            Data = "token",
            Expires = DateTime.UtcNow.AddMinutes(5),
        };

        _refreshToken = new Token()
        {
            Data = "refresh_token",
            Expires = DateTime.UtcNow.AddDays(3)
        };
        
        return true;
    }

    public bool Logout()
    {
        // Retrieve the token
        // Forget the token
        _authToken = null;
        _refreshToken = null;
        return true;
    }
}