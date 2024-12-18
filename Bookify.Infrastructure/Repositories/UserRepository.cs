using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext)
    : Repository<User>(dbContext), IUserRepository
{
    
}