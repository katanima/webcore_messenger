using Microsoft.OpenApi.Models;
using webcore_backend.Features.Auth.Services;
using webcore_backend.Features.Friends.Services;
using webcore_backend.Features.Users.Services;

namespace webcore_backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRelationshipService, RelationshipService>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "webcore_backend", Version = "v1", Description = "" });
        });
        
        return services;
    }
}