namespace vPicETL
{

  public record YearMo(int Year, int Month)
  {
    public static YearMo ThisMo
      => new YearMo(DateTime.UtcNow.Year, DateTime.UtcNow.Month);

    public static YearMo LastMo
      => new YearMo(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month);

  }

}
