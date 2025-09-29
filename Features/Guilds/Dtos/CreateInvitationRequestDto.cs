namespace webcore_backend.Features.Guilds.Dtos;

public record CreateInvitationRequestDto(
    Guid GuildId,
    DateTime? ExpirationDate,
    int? Usage
    );