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
    
    public HashSet<int> triggeredDates = new() { 1, 2, 3 };
    
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
        var checkingTime = currentTime.AddDays(-3);
        var users = await _baseRepository.GetUsersPayedUntilAsync(checkingTime);
        bool changes = false;

        var notifiesLift = new List<Notification>();
        
        foreach (var user in users)
        {
            var different = user.PayedUntil - currentTime;
            if (different.Days < 0)
            {
                user.RateId = 1;
                user.PayedUntil = null;
                await _rateManager.ChangeRate(user, saveChanges: false);
                notifiesLift.Add(new Notification()
                {
                    Type = Notification.NotificationType.RateExpired,
                    User = user
                });
                
                changes = true;
            }

            if (triggeredDates.Contains(different.Days))
            {
                notifiesLift.Add(new Notification()
                {
                    Type = Notification.NotificationType.RateExpiration,
                    User = user,
                    ExtraData = new Dictionary<string, object>()
                    {
                        ["DaysLeft"] = different.Days,
                    }
                });
            }
        }

        if (changes)
            await _baseRepository.SaveContextAsync();

        _notifyManager.ExecuteAndForget(notifiesLift);
        
        PlannedTime = currentTime.AddHours(24);
        planner.AddTask(this);
    }
}