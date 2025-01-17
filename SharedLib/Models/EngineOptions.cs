namespace vPic.SharedLib.IO
{
  public partial class VPicSqlDbCtx
  {
    public record EngineOptions(
      string Keys, 
      string EngineModel, 
      string FuelType,
      string DisplacementL,
      string EngineConfiguration,
      string NoCylinders,
      string ValveTrainDesign,
      string Turbo,
      string OtherEngineInfo
    );
  }
}
