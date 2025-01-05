using System.Security.Claims;
using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Infrastructure.Authorization;

public sealed class CustomeClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
            return principal;

        using var scope = serviceProvider.CreateScope();
        var authorizationService = serviceProvider.GetRequiredService<AuthorizationService>();
        var identityId = principal.GetIdentityId();
        var userRoles = await authorizationService.GetRolesForUserAsync(identityId);
        if (userRoles is null) return principal;
        
        ClaimsIdentity claimsIdentity = new();
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));
        userRoles.Roles.ForEach(role => claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name)));
        
        principal.AddIdentity(claimsIdentity);
        
        return principal;
    }
}