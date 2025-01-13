using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using vPicETL;
using vPicETL.IO;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<VPicSyncSvc>();
builder.Services.AddSingleton<FileStoreDb>();
builder.Services.AddHttpClient<VpicNhtsaClient>();

using IHost host = builder.Build();

var svc = host.Services.GetRequiredService<VPicSyncSvc>();
await svc.EnsureVPicDownloadedAsync();

Console.WriteLine("FINISHED");

await host.RunAsync();

