namespace CShroudApp.Infrastructure.Data.Config;

public class PathConfig
{
    public PathCoresConfig Cores { get; set; } = new();
}

public class PathCoresConfig
{
    public PathCorePackConfig SingBox { get; set; } = new() {Path = "/Binaries/Cores/SingBox"};
}

public class PathCorePackConfig
{
    public required string Path { get; set; }
    public string Args { get; set; } = "";
}