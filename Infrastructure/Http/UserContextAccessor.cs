using System.Security.Claims;

namespace webcore_backend.Infrastructure.Http;

public class UserContextAccessor(IHttpContextAccessor _accessor) : IUserContextAccessor
{
    public Guid Id()
        => Guid.Parse(
            _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException("No valid token provided")
        );
}