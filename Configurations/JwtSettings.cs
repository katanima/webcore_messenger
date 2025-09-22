using System.ComponentModel.DataAnnotations;

namespace webcore_backend.Configurations;

public class JwtSettings
{
    [Required(ErrorMessage = "JWT ExpiryHours must be set")]
    public int ExpiryHours { get; set; } = 0;

    [Required] public string Key { get; set; } = null!;
    [Required] public string Issuer { get; set; } = null!;
    [Required] public string Audience { get; set; } = null!;

    public JwtSettings() {}
}
