using Microsoft.AspNetCore.Identity;
using webcore_backend.Features.Users.Core.Dtos;
using webcore_backend.Models;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Features.Users.Core.Services;

public class UserService(AppDbContext _dbContext) : IUserService
{
    private readonly PasswordHasher<UserEntity> _hasher = new();

    public async Task<Guid> RegisterUserAsync(RegisterUserRequestDto dto)
    {
        var user = new UserEntity
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = _hasher.HashPassword(new UserEntity(), dto.Password)
        };

        await _dbContext.User.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<GetUserResponseDto> GetUserAsync(Guid userId)
    {
        var currentUser = await _dbContext.User.FindAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found");
        
        return GetUserResponseDto.From(currentUser);
    }

    /*public Task<bool> VerifyPasswordAsync(UserEntity user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return Task.FromResult(result == PasswordVerificationResult.Success);
    }*/

    /*public async Task<UserEntity> RequireUserByIdAsync(Guid userId)
        => await _dbContext.User.FindAsync(userId)
               ?? throw new UnauthorizedAccessException("User not found");

    public async Task<UserEntity?> FindUserByEmailAsync(string email) 
        => await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity?> FindUserByUsernameAsync(string username) 
        => await _dbContext.User.FirstOrDefaultAsync(u => u.Username == username);*/
}