using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Services;

public interface IScoresService
{
    Task<IEnumerable<ScoresDto>> GetScores();
}