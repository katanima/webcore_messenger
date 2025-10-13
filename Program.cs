using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webcore_backend.Configurations;
using webcore_backend.Extensions;
using webcore_backend.Models;

#region Database configuration load
Env.Load();
string[] dbPasswordPossiblePaths = ["/run/secrets/db_password", "./db_password.txt"];
var dbPasswordPath = dbPasswordPossiblePaths.FirstOrDefault(File.Exists);
var dbPassword = dbPasswordPath != null ? File.ReadAllText(dbPasswordPath).Trim() : "";
var databaseSettings = new DbSettings(
    Host: Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "postgres",
    Port: Environment.GetEnvironmentVariable("POSTGRES_PORT"),
    Name: Environment.GetEnvironmentVariable("POSTGRES_DB"),
    Username: Environment.GetEnvironmentVariable("POSTGRES_USER"),
    Password: dbPassword
);
#endregion

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
#region Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("User", new() { Title = "User", Description = "", Version = "V1" });
    options.SwaggerDoc("Guild", new() { Title = "Guild", Description = "", Version = "V1" });
});
#endregion
builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();
builder.Services.AddAppServices();
#region Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(databaseSettings.ToConnectionString())
);
#endregion
#region Authentication configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });
builder.Services.AddAuthorization();
#endregion
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});


var app = builder.Build();
#region Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/User/swagger.json", "User");
    options.SwaggerEndpoint("/swagger/Guild/swagger.json", "Guild");
    options.RoutePrefix = string.Empty;
});
#endregion
if (app.Environment.IsProduction())
{
    app.UseHsts();
}
#region Database middleware
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
        Console.WriteLine((created) ? "Database and tables created successfully!" : "Database already exists.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
        Console.WriteLine("Make sure PostgreSQL is running on localhost:5432 or run 'docker-compose up -d postgres'");
    }
}
#endregion
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
