namespace CShroudApp.Infrastructure.Data.Config;

public class PathConfig
{
    public PathCoresConfig Cores { get; set; } = new();
}

public class PathCoresConfig
{
    public string SingBox { get; set; } = "/Binaries/Cores/SingBox";
}