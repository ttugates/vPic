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
    
    /// <exception cref="Exception"></exception>
    public async Task<bool> DbFileExistsAsync(YearMo date)
    {
      var url = GetSourceURL(date);

      var res = await httpClient.SendAsync(
        new HttpRequestMessage(HttpMethod.Head, url));

      if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
        return false;

      res.EnsureSuccessStatusCode();      
          
      return true;
    }

    /// <exception cref="Exception"></exception>
    public async Task<Stream> DownloadDbFileAsync(YearMo date)
    {
      var url = GetSourceURL(date);
      return await httpClient.GetStreamAsync(url);
    } 
    
  }
}
