using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext dbContext)
{
    public async Task<UserRolesResponse?> GetRolesForUserAsync(string identityId)
    {
        var roles = await dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)?
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }) 
            .FirstOrDefaultAsync()!;

         return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var permissions = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permissionSet = permissions.Select(p => p.Name).ToHashSet();
        
        return permissionSet;
    }
}