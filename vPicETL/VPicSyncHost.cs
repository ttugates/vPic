using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using vPic.ETL.IO;
using vPic.SharedLib.IO;
using vPic.SharedLib.Models;


namespace vPic.ETL
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
      CancellationTokenSource? _cancellationTokenSource = null;

      appLifetime.ApplicationStarted.Register(() =>
      {
        logger.LogDebug("Application has started");
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _applicationTask = Task.Run(async () =>
        {
          try
          {
            // await EnsureVPicDownloadedAsync(cancellationToken);
            await ValidateDataAssumptionsBruteAsync();
            _exitCode = 0;
          }
          catch (TaskCanceledException)
          {
            // This means the application is shutting down
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "Unhandled exception!");
            _exitCode = 1;
          }
          finally
          {
            Console.WriteLine("FINISHED: Press any key to exit...");
            Console.ReadKey();
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

    /// <exception cref="Exception"></exception>
    private async Task EnsureVPicDownloadedAsync(CancellationToken cancellationToken, bool overwrite = false)
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

    /// <exception cref="Exception"></exception>
    private async Task<bool> EnsureLoadFromSrcAsync(YearMo date, CancellationToken cancellationToken, bool overwrite = false)
    {
      var dbExistsLocal = await vPicSqlDbCtx.DbExistsAsync(date);
      if (dbExistsLocal)
        return true;

      var bakFileExistsRemotely = await vpicNhtsaClient.DbFileExistsAsync(date);
      if (!bakFileExistsRemotely)
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

    private async Task ValidateDataAssumptionsBruteAsync()
    {
      var errors = new List<(string YMM, int pKeysCount, int vsIdsCount)>();
      int i = 0;

      var years = await vPicSqlDbCtx.GetYearsAsync();
      foreach (var year in years)
      {
        var makes = await vPicSqlDbCtx.GetMakesAsync(year);
        foreach (var make in makes)
        {
          var models = await vPicSqlDbCtx.GetModelsAsync(year, make.Id);
          foreach (var model in models)
          {
            var vinSchemaId = await vPicSqlDbCtx.GetVinSchemaIdsAsync(year, make.Id, model.Id);
            var vsIds = vinSchemaId.Select(x => x.VinSchemAId).Distinct().ToList();
            var pIds = vinSchemaId.Select(x => x.PatternKeys).Distinct().ToList();

            i++;
            if (vsIds.Count != 1)
              errors.Add(($"{year}-{make.Id}-{model.Id}", pIds.Count, vsIds.Count));

            if (i % 250 == 0)
            {
              errors = errors.OrderBy(x => x.pKeysCount + x.vsIdsCount).ToList();
               Console.WriteLine($"Processed: {i}, Errors: {errors.Count}");
            }
          }
        }

      }
    }
  }
}
