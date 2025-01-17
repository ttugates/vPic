namespace vPic.SharedLib.Models
{
  public record VehicleSpec(string GroupName, string Variable, string AttributeId, int? ElementId, string? Value);

  public record LooseVehicleSpec()
  {
    public string? GroupName { get; set; }
    public string Variable { get; set; }
    public string AttributeId { get; set; }
    public int? ElementId { get; set; }
    public string? Value { get; set; }
  };
}
