using System.Text;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using webcore_backend.Configurations;
using webcore_backend.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using webcore_backend.Extensions;

Env.Load();

string[] dbPasswordPossiblePaths = ["/run/secrets/db_password", "./db_password.txt"];
var dbPasswordPath = dbPasswordPossiblePaths.FirstOrDefault(File.Exists);
var dbPassword = dbPasswordPath != null ? File.ReadAllText(dbPasswordPath).Trim() : null;

var databaseSettings = new DbSettings(
    Host: Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "postgres",
    Port: Environment.GetEnvironmentVariable("POSTGRES_PORT"),
    Name: Environment.GetEnvironmentVariable("POSTGRES_DB"),
    Username: Environment.GetEnvironmentVariable("POSTGRES_USER"),
    Password: dbPassword
);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();
builder.Services.AddAppServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(databaseSettings.ToConnectionString())
);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});


var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        Console.WriteLine($"Attempting to connect to database...");
        Console.WriteLine($"Connection string: {databaseSettings.ToConnectionString()}");
        
        await context.Database.CanConnectAsync();
        Console.WriteLine("Database connection successful!");
        
        var created = await context.Database.EnsureCreatedAsync();
        Console.WriteLine((created)? "Database and tables created successfully!" : "Database already exists.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
        Console.WriteLine("Make sure PostgreSQL is running on localhost:5432 or run 'docker-compose up -d postgres'");
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
