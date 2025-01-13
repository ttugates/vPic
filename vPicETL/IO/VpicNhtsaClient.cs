using Microsoft.Extensions.Logging;

namespace vPicETL.IO
{
  public class VpicNhtsaClient(HttpClient httpClient, ILogger<VpicNhtsaClient> logger)
  {
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

    public async Task<Stream> GetDbFileAsync(YearMo date)
    {
      var url = GetSourceURL(date);
      return await httpClient.GetStreamAsync(url);
    }

    public static string GetFileName(YearMo date)
    {
      return $"vPICList_lite_{date.Year}_{date.Month}.bak.zip";
    }

    private static string GetSourceURL(YearMo date)
    {
      var fileName = GetFileName(date);
      return "https://vpic.nhtsa.dot.gov/api/" + fileName;
    }

  }
}
