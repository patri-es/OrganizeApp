using Backend.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // url de react
                                              .AllowAnyMethod()
                                              .AllowAnyHeader();
                      });
});

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

app.UseCors(MyAllowSpecificOrigins);
// mapea controladores para la API
app.MapControllers();

app.Run();
