namespace webcore_backend.Features.Guilds.Core.Dtos;

public record CreateGuildRequestDto(
    string Name,
    string? Description
    );