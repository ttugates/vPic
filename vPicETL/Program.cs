using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using vPicETL;
using vPicETL.IO;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<VPicSqlDbCtx>();
builder.Services.AddSingleton<FileStoreDb>();
builder.Services.AddHttpClient<VpicNhtsaClient>();
builder.Services.AddHostedService<VPicSyncHost>();

using IHost host = builder.Build();
await host.RunAsync();

