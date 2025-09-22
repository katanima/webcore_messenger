using System.ComponentModel.DataAnnotations;

namespace webcore_backend.Configurations;

public record DbSettings(
    [Required] string Host,
    [Required] string Port,
    [Required] string Name,
    [Required] string Username,
    string? Password
)
{
    public string ToConnectionString()
        => $"Host={Host};Port={Port};Database={Name};Username={Username};Password={Password}";
}