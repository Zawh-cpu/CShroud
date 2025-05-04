using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Infrastructure.Services;
using CShroudGateway.Infrastructure.Tasks;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway;

internal static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString),
            ServiceLifetime.Scoped);

        builder.Services.AddHttpClient("TelegramHook",
            client => client.BaseAddress = new Uri("https://api.test.org/"));

        builder.Services.AddScoped<IBaseRepository, BaseRepository>();

        builder.Services.AddSingleton<IGrpcPool, GrpcPool>();
        builder.Services.AddSingleton<IUpdatePrimitive, UpdatePrimitive>();
        builder.Services.AddSingleton<INotificationManager, NotificationManager>();
        builder.Services.AddSingleton<IPlanner, Planner>();
        builder.Services.AddScoped<IVpnRepository, VpnRepository>();
        builder.Services.AddScoped<IVpnService, VpnService>();
        builder.Services.AddScoped<IVpnKeyService, VpnKeyService>();
        builder.Services.AddScoped<IVpnServerManager, VpnServerManager>();
        builder.Services.AddScoped<IVpnStorage, VpnStorage>();
        builder.Services.AddScoped<IRateManager, RateManager>();

        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // app.UseHttpsRedirection();
        app.MapGrpcService<ControlService>();
        app.MapGrpcService<UpdateService>();
        app.MapGrpcService<MachineService>();

        Task.WhenEach(CheckForReservedConstants(app.Services));
        RunTasks(app.Services);

        app.Run();


        return 0;
    }

    public static async Task CheckForReservedConstants(IServiceProvider serviceProvider)
    {
        var baseRepository = serviceProvider.GetRequiredService<IBaseRepository>();

        if (await baseRepository.GetUserByIdAsync(ReservedUsers.System) == null)
        {
            var user = new User()
            {
                Id = ReservedUsers.System,
                IsActive = true,
                Nickname = "System",
                IsVerified = true,
            };

            await baseRepository.AddWithSaveAsync(user);
        }
    }

    public static void RunTasks(IServiceProvider serviceProvider)
    {
        var planner = serviceProvider.GetRequiredService<IPlanner>();

        planner.AddTask(new PaymentsCheckTask(DateTime.UtcNow.AddMinutes(5)));
    }
}