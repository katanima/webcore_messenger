using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Friends.Entity;
using webcore_backend.Features.Users.Entities;

namespace webcore_backend.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }
    public DbSet<RelationshipEntity> Relationship { get; set; }
}