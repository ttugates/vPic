﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;
using vPicETL.Models;

namespace vPicETL.IO
{
  public class VPicSqlDbCtx(ILogger<VPicSqlDbCtx> logger)
  {
    private const string ConnStr 
      = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=master;Integrated Security=SSPI;TrustServerCertificate=True";
    
    private string GetVPicDataDBName(YearMo date) 
      => $"VPicData_{date.Year}_{date.Month:D2}";

    public async Task CreateDBAsync(YearMo date, string sourceFilePath)
    {
      var dbName = GetVPicDataDBName(date);
  
      try
      {
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
      catch (Exception ex)
      {
        logger.LogError(ex, "Error executing Create DB: ");
        ExceptionDispatchInfo.Capture(ex).Throw();
        throw;
      }

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
