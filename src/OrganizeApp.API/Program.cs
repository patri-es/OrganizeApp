using OrganizeApp.API.Exceptions;
using OrganizeApp.Application;
using OrganizeApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// exceptions and errors
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();
// Middleware: API uses Https, Tests uses Http
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}
// CORS
app.UseCors("_myAllowSpecificOrigins");
//app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the Program class public using a partial class declaration by test
public partial class Program { }