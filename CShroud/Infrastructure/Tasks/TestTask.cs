using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Tasks;

public class TestTask : IPlannedTask
{
    public DateTime PlannedTime { get; set; }

    public TestTask(DateTime plannedTime)
    {
        PlannedTime = plannedTime;
    }

    public void Action(IPlanner planner)
    {
        Console.WriteLine("Action");
    }
}