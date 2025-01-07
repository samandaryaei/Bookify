namespace Bookify.Infrastructure.Outbox;

public sealed class OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
{
    public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;
    public string Type { get; init; } = type;
    public string Content { get; init; } = content; //Json string will be fill
    public Guid Id { get; init; } = id;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}