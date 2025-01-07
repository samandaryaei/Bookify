using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bookify.Infrastructure
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            // var configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.Development.json")
            //     .Build();

            // Get connection string
            var connectionString = @"Host=bookify-db;Port=5432;DataBase=bookify;Username=postgres;Password=postgres;";//configuration.GetConnectionString("DataBase");

            // Create options for DbContext
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

            // Provide a mock or dummy IPublisher instance
            //var mockPublisher = new MockPublisher();

            return new ApplicationDbContext(optionsBuilder.Options, null);
        }
    }

    // A basic mock implementation of IPublisher for migrations
    public class MockPublisher : IPublisher
    {
        // Implement the generic Publish<TNotification>
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            // No-op for design-time purposes
            return Task.CompletedTask;
        }

        // Implement the non-generic Publish (if required)
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            // No-op for design-time purposes
            return Task.CompletedTask;
        }
    }
}