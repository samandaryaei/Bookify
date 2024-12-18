using Bookify.Domain.Apartments;

namespace Bookify.Infrastructure.Repositories;

public sealed class ApartmentRepository(ApplicationDbContext dbContext)
    : Repository<Apartment>(dbContext), IApartmentRepository
{
    
}