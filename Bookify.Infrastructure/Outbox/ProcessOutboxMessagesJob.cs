using System.Data;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox message job");
        
        using var connection = sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(transaction, connection);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent
                    = JsonConvert.DeserializeObject<IDomainEvents>(outboxMessage.Content,
                        JsonSerializerSettings);

                await publisher.Publish(domainEvent!, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException,
                    "Exception caught while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();
        
        logger.LogInformation("Completed processing outbox message ");
    }

    private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction,
        OutboxMessageResponse outboxMessageResponse, Exception? exception)
    {
        const string sql = $"""
                            UPDATE outbox_messages
                            SET processed_on_utc = @ProcessedOnUtc,
                                error = @Error
                            WHERE id = @Id
                            """;
        await connection.ExecuteAsync(sql,
            new
            {
                outboxMessageResponse.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync
        (IDbTransaction transaction, IDbConnection connection)
    {
        var sql = $"""
                   SELECT id,content
                   FROM outbox_messages
                   WHERE processed_on_utc IS NULL
                   ORDER BY processed_on_utc
                   LIMIT {_outboxOptions.BatchSize}
                   FOR UPDATE
                   """;

        var outboxMessages
            = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }
}