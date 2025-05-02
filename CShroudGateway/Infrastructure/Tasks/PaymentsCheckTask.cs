using System.Collections;
using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Tasks;

public class PaymentsCheckTask : IPlannedTask
{
    public DateTime PlannedTime { get; set; }
    
    private IBaseRepository _baseRepository;
    private IVpnKeyService _keyService;
    private IRateManager _rateManager;
    private INotificationManager _notifyManager;
    
    public HashSet<int> triggeredDates = new() { 0, 1, 3 };
    
    public PaymentsCheckTask(DateTime plannedTime, IBaseRepository baseRepository, IVpnKeyService keyService, IRateManager rateManager, INotificationManager notifyManager)
    {
        PlannedTime = plannedTime;
        _baseRepository = baseRepository;
        _keyService = keyService;
        _rateManager = rateManager;
        _notifyManager = notifyManager;
    }

    public virtual async Task Action(IPlanner planner, DateTime currentTime)
    {
        var users = await _baseRepository.GetUsersPayedUntilAsync(x => currentTime.AddDays(-3) <= x.PayedUntil && x.PayedUntil <= currentTime);
        var expiredUsers = await _baseRepository.GetUsersPayedUntilAsync(x => x.PayedUntil <= currentTime.AddDays(1));

        var notifiesLift = new List<Notification>();

        foreach (var user in expiredUsers)
        {
            user.RateId = 1;
            await _rateManager.ChangeRateAsync(user, saveChanges: false);
            notifiesLift.Add(new Notification()
            {
                Type = Notification.NotificationType.RateExpired,
                User = user
            });
        }

        if (users.Any())
            await _baseRepository.SaveContextAsync();

        foreach (var user in users)
        {
            if (user.PayedUntil is null ) continue;

            notifiesLift.Add(new Notification()
            {
                Type = Notification.NotificationType.RateExpiration,
                User = user,
                ExtraData = new Dictionary<string, object>()
                {
                    ["DaysLeft"] = user.PayedUntil - currentTime
                }
            });
        }

        _notifyManager.ExecuteAndForget(notifiesLift);
        
        PlannedTime = currentTime.AddHours(24);
        planner.AddTask(this);
    }
}