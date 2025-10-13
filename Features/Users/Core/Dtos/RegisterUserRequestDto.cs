namespace webcore_backend.Features.Users.Core.Dtos;

public record RegisterUserRequestDto(
    string Username,
    string Email,
    string Password
    );