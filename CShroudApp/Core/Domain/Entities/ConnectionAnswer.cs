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