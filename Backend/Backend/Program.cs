var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Services
builder.Services.AddControllers();
//app.MapGet("/", () => "Hello World!");

app.Run();
