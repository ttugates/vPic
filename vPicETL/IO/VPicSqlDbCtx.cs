using Dapper;
using Microsoft.Data.SqlClient;

namespace vPicETL.IO
{
  public class VPicSqlDbCtx
  {

    public const string ConnectionString = 
      """
      Data Source=localhost\\SQLEXPRESS;Initial Catalog=master;Integrated Security=SSPI;TrustServerCertificate=True
      """;

    public async Task TestIOAsync()
    {

      using var conn = new SqlConnection(ConnectionString);
      conn.Open();
      var res = conn.ExecuteScalar<DateTime>("SELECT GETDATE();");
      Console.WriteLine(res.ToString());

    }
  }
}
