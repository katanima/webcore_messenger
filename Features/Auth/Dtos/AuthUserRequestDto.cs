namespace webcore_backend.Features.Auth.Dtos;

public record AuthUserRequestDto(
    string? Username,
    string? Email,
    string Password
    );