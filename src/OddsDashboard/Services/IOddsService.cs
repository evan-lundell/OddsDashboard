using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;
public interface IOddsService
{
    Task<IEnumerable<OddsDto>> GetOdds();
}