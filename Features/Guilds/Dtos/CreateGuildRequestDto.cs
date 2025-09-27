namespace webcore_backend.Features.Guilds.Dtos;

public record CreateGuildRequestDto(
    string Name,
    string? Description
    );