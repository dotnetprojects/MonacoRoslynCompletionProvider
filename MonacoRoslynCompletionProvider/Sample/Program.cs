using Microsoft.Extensions.FileProviders;
using Sample;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (e) => {
    var path = e.Request.Path.Value;
    if (path == "/")
        path = "/index.html";
    var text = File.ReadAllText("html" + path);
    var bytes = System.Text.Encoding.UTF8.GetBytes(text);
    await e.Response.Body.WriteAsync(bytes);
});

app.MapPost("/completion", async (e) =>
{
    var requestBody = e.Request.Body;
    StreamReader reader = new StreamReader(requestBody);
    string text = await reader.ReadToEndAsync();
    if (text != null)
    {
        TabCompletionRequest completionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
        var tabCompletionDTO = await MonacoRoslynCompletionProvider.CompletionProvider.GetCodeCompletionForCode(completionRequest.Code, completionRequest.Position, completionRequest.Assemblies);
        string jsonString = JsonSerializer.Serialize(tabCompletionDTO);
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
        await e.Response.Body.WriteAsync(bytes);
    } else
    {
        e.Response.StatusCode = 405;
        await e.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("Invalid Request"));
    }
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"html/node_modules")),
    RequestPath = new PathString("/nm"),
    EnableDirectoryBrowsing = true
});
app.UseStaticFiles();

app.Run();
