using webcore_backend.Shared.Users.Dtos;

namespace webcore_backend.Features.Users.Relationships.Dtos;

public record GetFriendListResponseDto(
    IEnumerable<UserGeneralInformationsDto> Friends
    );