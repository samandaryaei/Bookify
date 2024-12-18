using Bookify.Application.Abstractions.Clock;

namespace Bookify.Infrastructure.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}