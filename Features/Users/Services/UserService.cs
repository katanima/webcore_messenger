using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using webcore_backend.Configurations;
using webcore_backend.Features.Users.Dtos;
using webcore_backend.Features.Users.Entities;
using webcore_backend.Infrastructure.Http;
using webcore_backend.Models;

namespace webcore_backend.Features.Users.Services;

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

    public async Task<GetUserResponseDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _dbContext.User.FindAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found");
        
        return GetUserResponseDto.From(user);
    }

    public async Task<UserEntity?> GetCurrentUserAsync(Guid userId)
        => await _dbContext.User.FindAsync(userId);
    
    public async Task<UserEntity?> GetUserByEmailAsync(string email)
        => await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        => await _dbContext.User.FirstOrDefaultAsync(u => u.Username == username);

    public Task<bool> VerifyPasswordAsync(UserEntity user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);;
        return Task.FromResult(result == PasswordVerificationResult.Success);
    }
}