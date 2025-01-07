namespace Bookify.Infrastructure.Outbox;

internal class OutboxOptions
{
    public int IntervalInSecond { get; set; }
    public int BatchSize { get; set; }
}