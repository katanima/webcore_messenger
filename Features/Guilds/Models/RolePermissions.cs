namespace webcore_backend.Features.Guilds.Models;

[Flags]
public enum RolePermissions
{
    None = 0,
    Administrator = 1 << 0,
    ManageInvites = 1 << 1,
    EditServer = 1 << 2,
}