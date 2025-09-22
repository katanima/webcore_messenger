using webcore_backend.Shared.Users.Dtos;

namespace webcore_backend.Features.Friends.Dtos;

public record GetFriendListResponseDto(
    IEnumerable<UserGeneralInformationsDto> Friends
    );