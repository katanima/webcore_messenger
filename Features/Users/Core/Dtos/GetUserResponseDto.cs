using Microsoft.AspNetCore.Identity;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Features.Users.Core.Dtos;

public record GetUserResponseDto(
    string Username,
    string Email,
    string? PhoneNumber
)
{
    private readonly PasswordHasher<UserEntity> _hasher = new();

    public static GetUserResponseDto From(UserEntity user)
        => new GetUserResponseDto(
            Username: user.Username,
            Email: user.Email,
            PhoneNumber: user.PhoneNumber
        );
}