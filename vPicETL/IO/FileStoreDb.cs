using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace vPicETL.IO
{
  public class FileStoreDb(ILogger<FileStoreDb> logger)
  {

    private static readonly string BaseDir =
      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "VPicEtl");

    public async void DecompressAndWriteBak(Stream s, YearMo date)
    {
      var path = EnsureFolderExists("Source");

      logger.LogInformation("BEGIN Writing file to folder: {path}", path);

      using var zip = new ZipArchive(s);
      foreach (var entry in zip.Entries)
      {
        var dest = Path.Combine(path, entry.Name);
        if (File.Exists(dest))
          File.Delete(dest);
        using var eachStr = entry.Open();
        using var dStr = File.OpenWrite(dest);
        await eachStr.CopyToAsync(dStr);
      }

      logger.LogInformation("FINISHED Writing to folder: {path}", path);

    }

    public bool DoesFileExist(string? relativeFolder, string fileName)
    {
      var path = BuildFullPath(relativeFolder, fileName);
      return File.Exists(path);
    }

    private string EnsureFolderExists(string? relativeFolder = null)
    {
      var path = string.IsNullOrWhiteSpace(relativeFolder)
        ? BaseDir
        : Path.Combine(BaseDir, relativeFolder);

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
        logger.LogInformation($"FINISHED Creating folder: {path}");        
      }

      return path;
    }

    public string BuildFullPath(string? relativeFolder, string fileName)
    {
      return string.IsNullOrWhiteSpace(relativeFolder)
        ? Path.Combine(BaseDir, fileName)
        : Path.Combine(BaseDir, Path.Combine(relativeFolder, fileName));
    }

  }

}
