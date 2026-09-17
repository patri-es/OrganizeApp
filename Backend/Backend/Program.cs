using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

// Database connection
string connectionString =
    builder.Configuration.GetConnectionString("Default")
    ?? throw new ArgumentNullException(
        "Connection String is not available"
    );

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));

// Build application
var app = builder.Build();
// mapea controladores para la API
app.MapControllers();

app.Run();
