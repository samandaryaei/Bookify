using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

public class HassPermissionAttribute(string permission) : AuthorizeAttribute(permission);