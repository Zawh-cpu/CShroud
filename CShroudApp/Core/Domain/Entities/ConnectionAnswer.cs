namespace CShroudApp.Core.Domain.Entities;

/*
{
    "protocol": "vless",
    "location": "frankfurt",
    "params": {},
    "data": {
        "settings": {
            "vnext": [
                {
                    "address": "frankfurt.reality.zawh.ru",
                    "port": 443,
                    "users": [
                        {
                            "id": "8d50da4e-fff4-4188-bbd1-7d620c7296f0",
                            "alterId": 0,
                            "email": "t@t.tt",
                            "security": "auto",
                            "encryption": "none",
                            "flow": "xtls-rprx-vision"
                        }
                    ]
                }
            ]
        },
        "streamSettings": {
            "network": "tcp",
            "security": "reality",
            "realitySettings": {
                "serverName": "google.com",
                "fingerprint": "random",
                "show": false,
                "publicKey": "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI",
                "shortId": "4ae60b64b5cd",
                "spiderX": "/"
            }
        },
        "mux": {
            "enabled": false,
            "concurrency": -1
        }
    },
    "obtained": "27 March 2024 - 20:11:10.2463"
}
 */

public class ConnectionAnswer
{
    public string Protocol = string.Empty;
    public string Location = string.Empty;
    public Dictionary<string, object> Params = new();
    public Dictionary<string, object> Data = new();
    public DateTime Obtained = DateTime.Now;
}

/*
{
    "settings": {
        "vnext": [
            {
                "address": "frankfurt.reality.zawh.ru",
                "port": 443,
                "users": [
                    {
                        "id": "8d50da4e-fff4-4188-bbd1-7d620c7296f0",
                        "alterId": 0,
                        "email": "t@t.tt",
                        "security": "auto",
                        "encryption": "none",
                        flow": "xtls-rprx-vision"
                    }
                ]
            }
        ]
    },
    "streamSettings": {
        "network": "tcp",
        "security": "reality",
        "realitySettings": {
            "serverName": "google.com",
            "fingerprint": "random",
            "show": false,
            "publicKey": "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI",
            "shortId": "4ae60b64b5cd",
            "spiderX": "/"
        }
    },
    "mux": {
        "enabled": false,
        "concurrency": -1
    }
}
*/
 
 
// ewogICAgInNldHRpbmdzIjogewogICAgICAgICJ2bmV4dCI6IFsKICAgICAgICAgICAgewogICAgICAgICAgICAgICAgImFkZHJlc3MiOiAiZnJhbmtmdXJ0LnJlYWxpdHkuemF3aC5ydSIsCiAgICAgICAgICAgICAgICAicG9ydCI6IDQ0MywKICAgICAgICAgICAgICAgICJ1c2VycyI6IFsKICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICJpZCI6ICI4ZDUwZGE0ZS1mZmY0LTQxODgtYmJkMS03ZDYyMGM3Mjk2ZjAiLAogICAgICAgICAgICAgICAgICAgICAgICAiYWx0ZXJJZCI6IDAsCiAgICAgICAgICAgICAgICAgICAgICAgICJlbWFpbCI6ICJ0QHQudHQiLAogICAgICAgICAgICAgICAgICAgICAgICAic2VjdXJpdHkiOiAiYXV0byIsCiAgICAgICAgICAgICAgICAgICAgICAgICJlbmNyeXB0aW9uIjogIm5vbmUiLAogICAgICAgICAgICAgICAgICAgICAgICBmbG93IjogInh0bHMtcnByeC12aXNpb24iCiAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgXQogICAgICAgICAgICB9CiAgICAgICAgXQogICAgfSwKICAgICJzdHJlYW1TZXR0aW5ncyI6IHsKICAgICAgICAibmV0d29yayI6ICJ0Y3AiLAogICAgICAgICJzZWN1cml0eSI6ICJyZWFsaXR5IiwKICAgICAgICAicmVhbGl0eVNldHRpbmdzIjogewogICAgICAgICAgICAic2VydmVyTmFtZSI6ICJnb29nbGUuY29tIiwKICAgICAgICAgICAgImZpbmdlcnByaW50IjogInJhbmRvbSIsCiAgICAgICAgICAgICJzaG93IjogZmFsc2UsCiAgICAgICAgICAgICJwdWJsaWNLZXkiOiAiOEFaUWxqYlNqdlBNUHZjaml6UE00SnBUbWNIQlBXeF9zdE1faDBnb2ZFSSIsCiAgICAgICAgICAgICJzaG9ydElkIjogIjRhZTYwYjY0YjVjZCIsCiAgICAgICAgICAgICJzcGlkZXJYIjogIi8iCiAgICAgICAgfQogICAgfSwKICAgICJtdXgiOiB7CiAgICAgICAgImVuYWJsZWQiOiBmYWxzZSwKICAgICAgICAiY29uY3VycmVuY3kiOiAtMQogICAgfQp9
// InNldHRpbmdzIjogeyJ2bmV4dCI6IFt7ImFkZHJlc3MiOiAiZnJhbmtmdXJ0LnJlYWxpdHkuemF3aC5ydSIsInBvcnQiOiA0NDMsInVzZXJzIjogW3siaWQiOiAiOGQ1MGRhNGUtZmZmNC00MTg4LWJiZDEtN2Q2MjBjNzI5NmYwIiwiYWx0ZXJJZCI6IDAsImVtYWlsIjogInRAdC50dCIsInNlY3VyaXR5IjogImF1dG8iLCJlbmNyeXB0aW9uIjogIm5vbmUiLGZsb3ciOiAieHRscy1ycHJ4LXZpc2lvbiJ9XX1dfSwic3RyZWFtU2V0dGluZ3MiOiB7Im5ldHdvcmsiOiAidGNwIiwic2VjdXJpdHkiOiAicmVhbGl0eSIsInJlYWxpdHlTZXR0aW5ncyI6IHsic2VydmVyTmFtZSI6ICJnb29nbGUuY29tIiwiZmluZ2VycHJpbnQiOiAicmFuZG9tIiwic2hvdyI6IGZhbHNlLCJwdWJsaWNLZXkiOiAiOEFaUWxqYlNqdlBNUHZjaml6UE00SnBUbWNIQlBXeF9zdE1faDBnb2ZFSSIsInNob3J0SWQiOiAiNGFlNjBiNjRiNWNkIiwic3BpZGVyWCI6ICIvIn19LCJtdXgiOiB7ImVuYWJsZWQiOiBmYWxzZSwiY29uY3VycmVuY3kiOiAtMX19

/*
{
"settings": {"vnext": [{"address": "frankfurt.reality.zawh.ru","port": 443,"users": [{"id": "8d50da4e-fff4-4188-bbd1-7d620c7296f0","alterId": 0,"email": "t@t.tt","security": "auto","encryption": "none",flow": "xtls-rprx-vision"}]}]},"streamSettings": {"network": "tcp","security": "reality","realitySettings": {"serverName": "google.com","fingerprint": "random","show": false,"publicKey": "8AZQljbSjvPMPvcjizPM4JpTmcHBPWx_stM_h0gofEI","shortId": "4ae60b64b5cd","spiderX": "/"}},"mux": {"enabled": false,"concurrency": -1}}
*/