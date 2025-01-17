using vPic.SharedLib.IO;

namespace API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateSlimBuilder(args);
      
      builder.Services.AddSingleton<VPicSqlDbCtx>();

      var app = builder.Build();

      var vPicApi = app.MapGroup("/vpic");

      vPicApi.MapGet("/years", 
        async (VPicSqlDbCtx dbCtx) 
          => await dbCtx.GetYearsAsync());

      vPicApi.MapGet("/makes/{year}", 
        async (VPicSqlDbCtx dbCtx, int year) 
          => await dbCtx.GetMakesAsync(year));

      vPicApi.MapGet("/models/{year}/{makeId}", 
        async (VPicSqlDbCtx dbCtx, int year, int makeId)
          => await dbCtx.GetModelsAsync(year, makeId));

      vPicApi.MapGet("/vehicle_specifications/{vin}", 
        async (VPicSqlDbCtx dbCtx, string vin)
          => await dbCtx.DecodeVinToVehicleSpecsAsync(vin));

      vPicApi.MapGet("/vin_schema_ids/{year}/{makeId}/{modelId}", 
        async (VPicSqlDbCtx dbCtx, int year, int makeId, int modelId)
          => await dbCtx.GetVinSchemaIdsAsync(year, makeId, modelId));

      vPicApi.MapGet("/engine_optoins/{vinSchemaId}/", 
        async (VPicSqlDbCtx dbCtx, int vinSchemaId)
          => await dbCtx.GetEngineOptionsAsync(vinSchemaId));

      app.Run();
    }
  }

}
