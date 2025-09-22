using webcore_backend.Features.Auth.Services;
using webcore_backend.Features.Friends.Services;
using webcore_backend.Features.Users.Services;

namespace webcore_backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRelationshipService, RelationshipService>();
        
        return services;
    }
}