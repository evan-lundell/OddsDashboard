using OddsDashboard.Services;
using OddsDashboard.Shared.ViewModels;

namespace OddsDashboard.Workers;

public class RefreshWorker(ILogger<RefreshWorker> logger, IRefreshService refreshService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RefreshWorker started");

        await refreshService.RefreshDashboardData();
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await refreshService.RefreshDashboardData();
        }
        
        logger.LogInformation("RefreshWorker is stopping");
    }
}