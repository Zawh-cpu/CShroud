using System.ComponentModel.DataAnnotations;

namespace CShroudGateway.Infrastructure.Data.Entities;

public class Server
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Location { get; set; }
    
    [StringLength(15)]
    public required string IpV4Address { get; set; }

    public List<Protocol> SupportedProtocols { get; set; } = new();
}