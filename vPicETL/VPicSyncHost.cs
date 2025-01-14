﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using vPicETL.IO;
using vPicETL.Models;

namespace vPicETL
{
  public class VPicSyncHost(
    IHostApplicationLifetime appLifetime,
    VpicNhtsaClient vpicNhtsaClient,
    ILogger<VPicSyncHost> logger,
    VPicSqlDbCtx vPicSqlDbCtx,
    FileStoreDb fileStoreDb
) : IHostedService
  {

    private int? _exitCode;
    private Task? _applicationTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
      logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

      CancellationTokenSource? _cancellationTokenSource = null;

      appLifetime.ApplicationStarted.Register(() =>
      {
        logger.LogDebug("Application has started");
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _applicationTask = Task.Run(async () =>
        {
          try
          {
            await EnsureVPicDownloadedAsync(cancellationToken);
            _exitCode = 0;
          }
          catch (TaskCanceledException)
          {
            // This means the application is shutting down, so just swallow this exception
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "Unhandled exception!");
            _exitCode = 1;
          }
          finally
          {
            Console.WriteLine("FINISHED: Press any key to exit...");
            Console.ReadLine();
            appLifetime.StopApplication();
          }
        });
      });

      appLifetime.ApplicationStopping.Register(() =>
      {
        logger.LogDebug("Application is stopping");
        _cancellationTokenSource?.Cancel();
      });     

      return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      // Wait for the application logic to fully complete any cleanup tasks.
      // Note that this relies on the cancellation token to be properly used in the application.
      if (_applicationTask != null)
      {
        await _applicationTask;
      }

      logger.LogDebug($"Exiting with return code: {_exitCode}");

      // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
      Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
    }

    public async Task EnsureVPicDownloadedAsync(CancellationToken cancellationToken, bool overwrite = false)
    {
      var currResSuccess = await EnsureLoadFromSrcAsync(YearMo.ThisMo, cancellationToken, overwrite);
      if (currResSuccess)
      {
        logger.LogInformation($"Data for {YearMo.ThisMo} is loaded");
        return;
      }
      var prevMoResSuccess = await EnsureLoadFromSrcAsync(YearMo.LastMo, cancellationToken, overwrite);
      if (prevMoResSuccess)
      {
        logger.LogInformation($"Data for {YearMo.LastMo} is loaded");
      }
      else
      {
        logger.LogWarning($"Failed to load data for {YearMo.ThisMo} and {YearMo.LastMo}");
      }
    }

    private async Task<bool> EnsureLoadFromSrcAsync(YearMo date, CancellationToken cancellationToken, bool overwrite = false)
    {
      var dbExistsLocal = await vPicSqlDbCtx.DbExistsAsync(date);
      if (dbExistsLocal) 
        return true;    
      
      var bakFileExistsRemotely = await vpicNhtsaClient.DbFileExistsAsync(date);
      if(!bakFileExistsRemotely) 
        return false;

      var bakFileExistsLocally = fileStoreDb.BakFileExists(date);
      if (!bakFileExistsLocally || overwrite)
      {   
        await using var sStr = await vpicNhtsaClient.DownloadDbFileAsync(date);
        await fileStoreDb.SaveBakAsync(sStr, date);        
      }

      var srcFile = fileStoreDb.BuildPathToBakFile(date);
      await vPicSqlDbCtx.CreateDBAsync(date, srcFile);

      return true;
    }
  }
}