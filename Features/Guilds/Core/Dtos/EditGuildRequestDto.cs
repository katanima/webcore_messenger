namespace webcore_backend.Features.Guilds.Core.Dtos;

public record EditGuildRequestDto(
    Guid GuildId,
    string? Name,
    string? Description
    );