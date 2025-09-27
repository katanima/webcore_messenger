namespace webcore_backend.Features.Guilds.Dtos;

public record EditGuildRequestDto(
    Guid GuildId,
    string? Name,
    string? Description
    );