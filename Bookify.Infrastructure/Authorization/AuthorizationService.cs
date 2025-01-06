using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService(ApplicationDbContext dbContext,ICacheService cacheService)
{
    public async Task<UserRolesResponse?> GetRolesForUserAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";
        var cacheRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey);
        
        if (cacheRoles is not null)
            return cacheRoles;
        
        var roles = await dbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)?
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }) 
            .FirstOrDefaultAsync()!;
        
        await cacheService.SetAsync<UserRolesResponse>(cacheKey, roles!);

         return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var chachekey = $"auth:permissions-{identityId}";
        var cachepermissions = await cacheService.GetAsync<HashSet<string>>(chachekey);
        
        if (cachepermissions is not null)
            return cachepermissions;
        
        var permissions = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permissionSet = permissions.Select(p => p.Name).ToHashSet();
        
        await cacheService.SetAsync<HashSet<string>>(chachekey, permissionSet);
        
        return permissionSet;
    }
}