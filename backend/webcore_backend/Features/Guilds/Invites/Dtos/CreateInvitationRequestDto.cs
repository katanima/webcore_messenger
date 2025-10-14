namespace webcore_backend.Features.Guilds.Invites.Dtos;

public record CreateInvitationRequestDto(
    Guid GuildId,
    DateTime? ExpirationDate,
    int? Usage
    );