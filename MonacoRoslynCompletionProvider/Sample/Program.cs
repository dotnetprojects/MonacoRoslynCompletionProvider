using MonacoRoslynCompletionProvider;
using MonacoRoslynCompletionProvider.Api;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/completion/{0}", async (e) =>
{
    using var reader = new StreamReader(e.Request.Body);
    string text = await reader.ReadToEndAsync();
    if (text != null)
    {
        if (e.Request.Path.Value?.EndsWith("complete") == true)
        {
            var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            var tabCompletionResults = await CompletitionRequestHandler.Handle(tabCompletionRequest);
            await JsonSerializer.SerializeAsync(e.Response.Body, tabCompletionResults);
            return;
        }
        else if (e.Request.Path.Value?.EndsWith("hover") == true)
        {
            var hoverInfoRequest = JsonSerializer.Deserialize<HoverInfoRequest>(text);
            var hoverInfoResult = await CompletitionRequestHandler.Handle(hoverInfoRequest);
            await JsonSerializer.SerializeAsync(e.Response.Body, hoverInfoResult);
            return;
        }
        else if (e.Request.Path.Value?.EndsWith("codeCheck") == true)
        {
            var codeCheckRequest = JsonSerializer.Deserialize<CodeCheckRequest>(text);
            var codeCheckResults = await CompletitionRequestHandler.Handle(codeCheckRequest);
            await JsonSerializer.SerializeAsync(e.Response.Body, codeCheckResults);
            return;
        }
    } 
    
    e.Response.StatusCode = 405;
});

app.UseFileServer();

app.Run();
