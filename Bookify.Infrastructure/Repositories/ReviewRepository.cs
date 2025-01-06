using Bookify.Domain.Reviews;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ReviewRepository(ApplicationDbContext dbContext)
    : Repository<Review>(dbContext), IReviewRepository;