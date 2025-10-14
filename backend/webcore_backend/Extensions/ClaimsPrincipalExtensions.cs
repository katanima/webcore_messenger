using System.Security.Claims;

namespace webcore_backend.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? user)
    {
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("No valid token provided");
        return Guid.Parse(userIdClaim);
    }
}