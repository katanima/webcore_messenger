namespace webcore_backend.Features.Users.Dtos;

public record RegisterUserRequestDto(
    string Username,
    string Email,
    string Password
    );