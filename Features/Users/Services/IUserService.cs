using webcore_backend.Features.Users.Dtos;
using webcore_backend.Features.Users.Entities;

namespace webcore_backend.Features.Users.Services;

public interface IUserService
{
    public Task<Guid> RegisterUserAsync(RegisterUserRequestDto dto);
    public Task<GetUserResponseDto> GetUserByIdAsync(Guid userId);

    public Task<UserEntity?> GetCurrentUserAsync(Guid userId);
    public Task<UserEntity?> GetUserByEmailAsync(string email);
    public Task<UserEntity?> GetUserByUsernameAsync(string username);
    public Task<bool> VerifyPasswordAsync(UserEntity user, string password);
}