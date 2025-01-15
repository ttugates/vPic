using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;
using vPic.SharedLib.Models;

namespace vPic.ETL.IO
{
  public class FileStoreDb(ILogger<FileStoreDb> logger)
  {
    private static readonly string BaseDir =
      Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "VPicEtl");

    private string FolderPath(string? relativeFolder = null)
      => string.IsNullOrWhiteSpace(relativeFolder)
          ? BaseDir
          : Path.Combine(BaseDir, relativeFolder);

    /// <exception cref="Exception"></exception>
    public async Task<string> SaveBakAsync(Stream s, YearMo date)
    {
      var folderPath = FolderPath("Source");
      EnsureFolderExists(folderPath);

      var filePath = BuildPathToBakFile(date);

      logger.LogInformation($"BEGIN Writing file to: {filePath}");

      using var zip = new ZipArchive(s);
      var zippedBak = zip.Entries.First(x => x.Name.EndsWith(".bak"));

      if (File.Exists(filePath))
        File.Delete(filePath);

      using var eachStr = zippedBak.Open();
      using var dStr = File.OpenWrite(filePath);

      await eachStr.CopyToAsync(dStr);

      logger.LogInformation($"FINISHED Writing file: {filePath}");

      return filePath;
    }

    /// <exception cref="Exception"></exception>
    public bool BakFileExists(YearMo date)
    {
      var path = BuildPathToBakFile(date);
      return File.Exists(path);
    }

    /// <exception cref="Exception"></exception>
    public string BuildPathToBakFile(YearMo date, bool ensureFolderExists = false)
    {
      var path = FolderPath("Source");
      if (ensureFolderExists)
        EnsureFolderExists("Source");

      return Path.Combine(path, date.ToString() + ".bak");

    }

    /// <exception cref="Exception"></exception>
    private void EnsureFolderExists(string folderPath)
    {
      if (Directory.Exists(folderPath))
        return;

      Directory.CreateDirectory(folderPath);

      var dInfo = new DirectoryInfo(folderPath);

      var dSecurity = dInfo.GetAccessControl();

      dSecurity.AddAccessRule(
        new FileSystemAccessRule(
          new SecurityIdentifier(WellKnownSidType.WorldSid, null),
          FileSystemRights.ReadAndExecute,
          InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
          PropagationFlags.NoPropagateInherit,
          AccessControlType.Allow));

      dInfo.SetAccessControl(dSecurity);

      logger.LogInformation($"FINISHED Creating folder: {folderPath}");

    }
  }

}
