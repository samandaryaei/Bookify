using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext)
    : Repository<User>(dbContext), IUserRepository
{
    public override void Add(User user)
    {
        foreach (var userRole in user.Roles)
        {
            DbContext.Attach(userRole);
        }
        base.Add(user);
    }
}