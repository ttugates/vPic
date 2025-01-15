using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using vPic.SharedLib.Models;


namespace vPic.SharedLib.IO
{
  public class VPicSqlDbCtx(ILogger<VPicSqlDbCtx> logger)
  {
    private const string ConnStr
      = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=master;Integrated Security=SSPI;TrustServerCertificate=True";

    private string GetVPicDataDBName(YearMo date)
      => $"VPicData_{date.Year}_{date.Month:D2}";

    private YearMo CurrentDb => new YearMo(2024, 12);
           
    public async Task<List<int>> GetYearsAsync()
    {
      var dbName = GetVPicDataDBName(CurrentDb);

      await using var con = await GetConnectionAsync();

      var sql = $"""
        SELECT DISTINCT(Year) FROM [{dbName}].[dbo].[VehicleSpecSchema_Year] ORDER BY [Year] DESC;
        """;

      var res = await con.QueryAsync<int>(sql);
      return res.AsList();
    }
           
    public async Task<List<Make>> GetMakesAsync(int year)
    {
      var dbName = GetVPicDataDBName(CurrentDb);

      await using var con = await GetConnectionAsync();

      var sql = $"""
        SELECT M.Id, M.Name
        FROM [{dbName}].[dbo].[VehicleSpecSchema] VSS
        INNER JOIN [{dbName}].[dbo].[VehicleSpecSchema_Year] VSY ON VSY.VehicleSpecSchemaId = VSS.Id
        INNER JOIN [{dbName}].[dbo].[Make] M ON VSS.MakeId = M.Id
        WHERE VSY.[Year] = @year 
            AND VSS.VehicleTypeId IN (2,3,7)
        GROUP BY M.Name, M.Id
        """;

      var res = await con.QueryAsync<Make>(sql, new { year });
      return res.AsList();
    }
       
    public async Task<List<Model>> GetModelsAsync(int year, int makeId)
    {
      var dbName = GetVPicDataDBName(CurrentDb);

      await using var con = await GetConnectionAsync();

      var sql = $"""
        SELECT MD.Id, MD.Name
        FROM [{dbName}].[dbo].[VehicleSpecSchema] VSS
        INNER JOIN [{dbName}].[dbo].[VehicleSpecSchema_Year] VSY ON VSY.VehicleSpecSchemaId = VSS.Id
        INNER JOIN [{dbName}].[dbo].[Make] M ON VSS.MakeId = M.Id
        INNER JOIN [{dbName}].[dbo].[VehicleSpecSchema_Model] VSMD ON VSMD.VehicleSpecSchemaId = VSS.Id
        INNER JOIN [{dbName}].[dbo].[Model] MD ON VSMD.ModelId = MD.Id
        WHERE VSY.[Year] = @year    
            AND VSS.VehicleTypeId IN (2,3,7)
            AND M.Id = @makeId
        GROUP BY MD.Name, MD.Id
        """;

      var res = await con.QueryAsync<Model>(sql, new {year, makeId });
      return res.AsList();
    }

    public async Task CreateDBAsync(YearMo date, string sourceFilePath)
    {
      var dbName = GetVPicDataDBName(date);

      await using var con = await GetConnectionAsync();

      string restoreSQL = $"""
          DECLARE @mdfLocation nvarchar(256) = CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS nvarchar(200)) + '{dbName}.mdf';

          DECLARE @ldfLocation nvarchar(256) = CAST(SERVERPROPERTY('InstanceDefaultLogPath') AS nvarchar(200)) + '{dbName}.ldf';

          RESTORE DATABASE {dbName}
          FROM DISK = '{sourceFilePath}'
          WITH
            MOVE 'vPICList_Lite1' TO @mdfLocation,
            MOVE 'vPICList_Lite1_log' TO @ldfLocation;
          """;

      logger.LogInformation("LOADING new SQL database: " + dbName);
      await con.ExecuteAsync(restoreSQL);
      logger.LogInformation("SUCCESS Created " + dbName);

    }

    public async Task<bool> DbExistsAsync(YearMo date)
    {
      var dbName = GetVPicDataDBName(date);
      var sql = $"""
        SELECT ISNULL((SELECT 1 FROM master.sys.databases WHERE name = N'{dbName}'), 0)
        """;

      await using var con = await GetConnectionAsync();
      return await con.ExecuteScalarAsync<bool>(sql);
    }

    private async Task<SqlConnection> GetConnectionAsync()
    {
      var con = new SqlConnection(ConnStr);
      await con.OpenAsync();
      return con;
    }
  }
}
