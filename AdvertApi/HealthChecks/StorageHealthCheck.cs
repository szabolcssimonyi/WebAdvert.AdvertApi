using System.Threading;
using System.Threading.Tasks;
using AdvertApi.Interfaces;
using Microsoft.Extensions.HealthChecks;

namespace AdvertApi.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService service;

        public StorageHealthCheck(IAdvertStorageService service)
        {
            this.service = service;
        }

        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            var isStorageOk = await service.CheckHealthAsync();
            return HealthCheckResult.FromStatus(isStorageOk ? CheckStatus.Healthy : CheckStatus.Unhealthy, string.Empty);
        }
    }
}
