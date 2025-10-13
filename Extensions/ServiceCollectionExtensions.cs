using Microsoft.OpenApi.Models;
using webcore_backend.Features.Auth.Services;
using webcore_backend.Features.Users.Core.Services;
using webcore_backend.Features.Users.Relationships.Services;

namespace webcore_backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRelationshipService, UserRelationshipService>();
        return services;
    }
}