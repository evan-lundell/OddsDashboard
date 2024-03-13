using OddsDashboard.Dtos;
using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;
public interface IOddsService
{
    Task<OddsResultDto> GetOdds();
}