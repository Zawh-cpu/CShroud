namespace CShroudApp.Core.Domain.Generators;

public class SingBox
{
    public static Dictionary<string, object> MakeDefaultConfig()
    {
        var result = new Dictionary<string, object>();
        
        result["dns"] = new List<Dictionary<string, object>>()
        {
            new()
            {
                {"tag", "block"},
                {"address", "rcode://success"}
            }
        };

        result["inbounds"] = new List<Dictionary<string, object>>();
        result["outbounds"] = new List<Dictionary<string, object>>();
        
        result["route"] = new Dictionary<string, object>()
        {
            {"auto_detect_interface", true},
            { "rules", new List<Dictionary<string, object>>() }
        };
        
        return result;
    }
}