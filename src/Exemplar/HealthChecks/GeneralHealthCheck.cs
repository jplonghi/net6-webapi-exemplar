using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Exemplar.HealthChecks
{
    public class GeneralHealthCheck : IHealthCheck
    {
        public  Task<HealthCheckResult> CheckHealthAsync( HealthCheckContext context, CancellationToken cancellationToken = default )
        {
            return  Task.FromResult (new HealthCheckResult( HealthStatus.Healthy, "Todo bien!" ));
        }
    }
}