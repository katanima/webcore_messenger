using webcore_backend.Features.Users.Core.Dtos;

namespace webcore_backend.Features.Users.Core.Services;

public interface IUserService
{
    public Task<Guid> RegisterUserAsync(RegisterUserRequestDto dto);
    public Task<GetUserResponseDto> GetUserAsync(Guid userId);
}