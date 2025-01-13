using Microsoft.Extensions.Logging;
using vPicETL.IO;

namespace vPicETL
{
  public class VPicSyncSvc(
    VpicNhtsaClient vpicNhtsaClient,
    VPicSqlDbCtx vPicSqlDbCtx,
    FileStoreDb fileStoreDb, 
    ILogger<VPicSyncSvc> logger)
  {

    public async Task EnsureVPicDownloadedAsync(bool overwrite = false)
    {

      await vPicSqlDbCtx.TestIOAsync();

      var currResSuccess = await EnsureLoadFromSrcAsync(YearMo.ThisMo, overwrite);
      if(currResSuccess)
        return;

      var prevMoResSuccess = await EnsureLoadFromSrcAsync(YearMo.LastMo, overwrite);    

    }

    private async Task<bool> EnsureLoadFromSrcAsync(YearMo date, bool overwrite = false)
    {
      var currMoFileName = VpicNhtsaClient.GetFileName(date);
      var currMoExistsLocal = fileStoreDb.DoesFileExist(null, currMoFileName);
      if (currMoExistsLocal && !overwrite)
        return true;

      var currMoExistsSrc = await vpicNhtsaClient.DbFileExistsAsync(date);
      if (currMoExistsSrc)      
        return await LoadFromSourceAsync(date);          

      return false;
    }

    private async Task<bool> LoadFromSourceAsync(YearMo date)
    {
      var fileName = VpicNhtsaClient.GetFileName(date);

      try
      { 
        await using var sStr = await vpicNhtsaClient.GetDbFileAsync(date);
        fileStoreDb.DecompressAndWriteBak(sStr, date);
        return true;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, $"Failed to download and write the file: {fileName}");
        return false;
      }
    }

  }
}
