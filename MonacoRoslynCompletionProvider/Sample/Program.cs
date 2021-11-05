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

app.Run();
