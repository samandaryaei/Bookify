namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvents> _domainEvents = new();
    protected Entity(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; init; }
    public IReadOnlyList<IDomainEvents> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvents domainEvent) => _domainEvents.Add(domainEvent);
}