namespace API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateSlimBuilder(args);

      var app = builder.Build();

      var vPicApi = app.MapGroup("/vpic");

      vPicApi.MapGet("/", () => Results.Ok("Years"));

      vPicApi.MapGet("/{id}", (int id) => Results.Ok(id));

      app.Run();
    }
  }

}
