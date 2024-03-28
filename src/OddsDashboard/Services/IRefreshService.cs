using OddsDashboard.Shared.ViewModels;

namespace OddsDashboard.Services;

public interface IRefreshService
{
    HomeViewModel HomeViewModel { get; }
    Task RefreshDashboardData();
    event Action<HomeViewModel>? DataRefreshed;
}