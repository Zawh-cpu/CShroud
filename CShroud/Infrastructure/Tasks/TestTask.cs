using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Tasks;

public class TestTask : IPlannedTask
{
    public DateTime PlannedTime { get; set; }
    private IVpnCore _vpnCore;

    public TestTask(DateTime plannedTime, IVpnCore vpnCore)
    {
        PlannedTime = plannedTime;
        _vpnCore = vpnCore;
    }

    public async Task Action(IPlanner planner, DateTime currentTime)
    {
        Console.WriteLine("SSSSS");
        Console.WriteLine($"Action: {_vpnCore.IsRunning}");
        planner.AddTask(new TestTask(currentTime.AddSeconds(5), _vpnCore));
        return;
    }
}