var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/current-time", () =>
{
    var currentDateTime = DateTime.Now.ToString();
    return Results.Text(currentDateTime);
});


app.Run();