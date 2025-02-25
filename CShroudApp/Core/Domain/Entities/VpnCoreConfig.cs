namespace CShroudApp.Core.Domain.Entities;

public class VpnCoreConfig
{
    public required string Path { get; set; }
    public required string ConfigPath { get; set; }
    public required string Arguments { get; set; }
    public required bool Debug { get; set; }
}