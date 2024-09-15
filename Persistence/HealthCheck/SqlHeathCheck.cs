using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence.HealthCheck
{
    public sealed class SqlHeathCheck : IHealthCheck
    {
        private readonly AppDbContext _dbContext;

        public SqlHeathCheck(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Perform a simple query to check the connectivity and availability of the SQL database
                var result = await _dbContext.Database.CanConnectAsync(cancellationToken);

                if (result)
                {
                    // The SQL database is healthy
                    return HealthCheckResult.Healthy();
                }
                else
                {
                    // The SQL database is unhealthy
                    return HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception ex)
            {
                // An exception occurred while checking the health of the SQL database
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }
}
