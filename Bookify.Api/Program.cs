using Bookify.Api.Extensions;
using Bookify.Api.Middlewares;
using Bookify.Api.OpenApi;
using Bookify.Application;
using Bookify.Application.Abstractions.Data;
using Bookify.Infrastructure;
using Dapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog(((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });

     // app.ApplyMigrations();
     // app.SeedData();
}

//app.UseHttpsRedirection();
app.UseCustomeExceptionHandler();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("health",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

// public class CustomeHealthCheck(ISqlConnectionFactory sqlConnectionFactory) : IHealthCheck
// {
//     public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             using var connection = sqlConnectionFactory.CreateConnection();
//             await connection.ExecuteScalarAsync("SELECT 1;");
//             
//             return HealthCheckResult.Healthy();
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             return HealthCheckResult.Unhealthy();
//         }
//     }
// }