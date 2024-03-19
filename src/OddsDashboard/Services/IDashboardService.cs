using OddsDashboard.Shared.ViewModels;

namespace OddsDashboard.Services;

public interface IDashboardService
{
    Task<HomeViewModel> GetDashboardData();
}