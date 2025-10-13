using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Guilds.Core.Entities;
using webcore_backend.Features.Guilds.Invites.Entities;
using webcore_backend.Features.Guilds.Members.Entities;
using webcore_backend.Features.Guilds.Roles.Entities;
using webcore_backend.Features.Users.Relationships.Entities;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }
    public DbSet<UserRelationshipEntity> Relationship { get; set; }
    public DbSet<GuildEntity> Guild { get; set; }
    public DbSet<GuildMemberEntity> GuildMember { get; set; }
    public DbSet<GuildInviteEntity> GuildInvite { get; set; }
    public DbSet<GuildRoleEntity> GuildRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .ToTable("Users");
        modelBuilder.Entity<UserRelationshipEntity>()
            .ToTable(tb => tb.HasCheckConstraint("CK_UserAId_LT_UserBId", "\"UserAId\" > \"UserBId\""))
            .HasKey(e => new { e.UserAId, e.UserBId });
            
        modelBuilder.Entity<GuildMemberEntity>()
            .HasKey(e => new { e.GuildId, e.UserId });
    }
}