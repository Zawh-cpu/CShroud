using System.Collections;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;

namespace CShroudGateway.Infrastructure.Tasks;


public class PaymentsCheckTask : IPlannedTask
{
    public HashSet<int> triggeredDates = new() { 0, 1, 3 };
    public DateTime PlannedDate { get; private set; }
    
    
    public PaymentsCheckTask(DateTime plannedTime)
    {
        PlannedDate = plannedTime;
    }

    public virtual async Task ActionAsync(IPlanner planner, DateTime currentTime, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var baseRepository = serviceProvider.GetRequiredService<IBaseRepository>();
        var rateManager = serviceProvider.GetRequiredService<IRateManager>();
        var notifyManager = serviceProvider.GetRequiredService<INotificationManager>();
        
        var users = await baseRepository.GetUsersPayedUntilAsync(x => currentTime.AddDays(-3) <= x.PayedUntil && x.PayedUntil <= currentTime);
        var expiredUsers = await baseRepository.GetUsersPayedUntilAsync(x => x.PayedUntil <= currentTime.AddDays(1));

        var notifiesLift = new List<Mail>();

        foreach (var user in expiredUsers)
        {
            user.RateId = 1;
            await rateManager.ChangeRateAsync(user, saveChanges: false);
            notifiesLift.Add(new Notification()
            {
                Type = Notification.NotificationType.RateExpired,
                User = user
            });
        }

        if (users.Any())
            await baseRepository.SaveContextAsync();

        foreach (var user in users)
        {
            if (user.PayedUntil is null ) continue;

            notifiesLift.Add(new Mail()
            {
                Type = MailType.RateExpiration,
                RecipientId = user.Id,
                SenderId = ReservedUsers.System,
                ExtraData = new Dictionary<string, object>()
                {
                    ["DaysLeft"] = user.PayedUntil - currentTime
                }
            });
        }

        notifyManager.ExecuteAndForget(notifiesLift);
        
        PlannedTime = currentTime.AddHours(24);
        planner.AddTask(this);
    }

    public DateTime PlannedTime { get; set; }
}