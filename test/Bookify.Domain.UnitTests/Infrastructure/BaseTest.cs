using Bookify.Domain.Abstractions;

namespace Bookify.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{
    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        var domainEvents = entity.GetDomainEvents().OfType<T>().SingleOrDefault();
        if (domainEvents is null)
            throw new Exception($"{typeof(T).Name} domain events were not published.");
        
        return domainEvents;
    }
}