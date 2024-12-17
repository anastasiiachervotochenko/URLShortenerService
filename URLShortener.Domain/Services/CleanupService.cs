using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using URLShortener.Domain.Contracts.Interfaces;

namespace URLShortener.Domain.Services;

public class CleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(60);

    public CleanupService(IServiceScopeFactory scopeFactory, ILogger<CleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cleanup Service is running.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IUrlService>();
                var deletedItemsCount = await service.CleanupExpiredUrlsAsync();
                _logger.LogInformation("Cleanup is finished at {datetime}; {deletedItemsCount} items were deleted",
                    DateTimeOffset.Now, deletedItemsCount);
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}