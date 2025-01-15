using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using vPic.ETL;
using vPic.ETL.IO;
using vPic.SharedLib.IO;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<VPicSqlDbCtx>();
builder.Services.AddSingleton<FileStoreDb>();
builder.Services.AddHttpClient<VpicNhtsaClient>();
builder.Services.AddHostedService<VPicSyncHost>();

using IHost host = builder.Build();
await host.RunAsync();

