using webcore_backend.Features.Users.Core.Dtos;

namespace webcore_backend.Features.Users.Core.Services;

public interface IUserService
{
    public Task<Guid> RegisterUserAsync(RegisterUserRequestDto dto);
    public Task<GetUserResponseDto> GetUserAsync(Guid userId);

    /*public Task<UserEntity> RequireUserByIdAsync(Guid userId);
    public Task<UserEntity?> FindUserByEmailAsync(string email);
    public Task<UserEntity?> FindUserByUsernameAsync(string username);*/
    /*public Task<bool> VerifyPasswordAsync(UserEntity user, string password);*/
}