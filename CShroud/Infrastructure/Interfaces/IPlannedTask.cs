namespace CShroud.Infrastructure.Interfaces;

public interface IPlannedTask
{
    DateTime PlannedTime { get; set; }
    void Action(IPlanner planner);
}