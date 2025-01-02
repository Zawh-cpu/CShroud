using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Tasks;

public class PlannedTask
{
    public DateTime PlannedTime;

    public PlannedTask(DateTime plannedTime)
    {
        PlannedTime = plannedTime;
    }

    public virtual void Action(IPlanner planner)
    {
    }
}