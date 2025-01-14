using Microsoft.Extensions.Logging;
using vPicETL.Models;

namespace vPicETL.IO
{
  public class VpicNhtsaClient(HttpClient httpClient, ILogger<VpicNhtsaClient> logger)
  {
    public static string GetFileName(YearMo date)
      => $"vPICList_lite_{date.Year}_{date.Month:D2}.bak.zip";
    
    private static string GetSourceURL(YearMo date)
      => "https://vpic.nhtsa.dot.gov/api/" + GetFileName(date);
    
    public async Task<bool> DbFileExistsAsync(YearMo date)
    {
      var url = GetSourceURL(date);
      try
      {
        var res = await httpClient.SendAsync(
          new HttpRequestMessage(HttpMethod.Head, url));

        return res.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "ERROR in executing DbFileExistsAsync");
        return false;
      }
    }

    public async Task<Stream> DownloadDbFileAsync(YearMo date)
    {
      var url = GetSourceURL(date);
      return await httpClient.GetStreamAsync(url);
    } 
    
  }
}
