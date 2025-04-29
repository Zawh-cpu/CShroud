using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data;
using CShroudGateway.Infrastructure.Services;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);

builder.Services.AddScoped<IBaseRepository, BaseRepository>();

builder.Services.AddSingleton<IGrpcPool, GrpcPool>();
builder.Services.AddSingleton<IUpdatePrimitive, UpdatePrimitive>();
builder.Services.AddScoped<IVpnRepository, VpnRepository>();
builder.Services.AddScoped<IVpnService, VpnService>();
builder.Services.AddScoped<IVpnKeyService, VpnKeyService>();
builder.Services.AddScoped<IVpnServerManager, VpnServerManager>();
builder.Services.AddScoped<IVpnStorage, VpnStorage>();

builder.Services.AddGrpc();


// builder.Services.AddOpenApi();

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

app.Run();
