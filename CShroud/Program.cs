using CShroud.Core.Domain.Entities;
using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Загрузка конфигурации из appsettings.json
builder.Services.Configure<VpnCoreConfig>(builder.Configuration.GetSection("VpnCore"));
builder.Services.AddSingleton<IProcessManager, ProcessManager>();
builder.Services.AddSingleton<IVpnCore, VpnCore>();

builder.Services.AddGrpc(); 

var app = builder.Build();

// var vpnCoreConfig = builder.Configuration.GetSection("VpnCore");
// Console.WriteLine($"Path: {vpnCoreConfig["Path"]}, Link: {vpnCoreConfig["Link"]}");


app.Run();