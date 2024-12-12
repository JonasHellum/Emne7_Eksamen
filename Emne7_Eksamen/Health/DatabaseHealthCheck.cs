using Emne7_Eksamen.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Emne7_Eksamen.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly GokstadAthleticsDbContext _dbContext;

    public DatabaseHealthCheck(GokstadAthleticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return HealthCheckResult.Healthy("Database connection is healthy");
            }
            else
            {
                return HealthCheckResult.Unhealthy("Database connection is not healthy");
            }
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("Database connection failed!", e);
        }
    }
}