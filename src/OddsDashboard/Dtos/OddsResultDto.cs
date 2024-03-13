using OddsDashboard.Shared.Dtos;

namespace OddsDashboard.Dtos;

public class OddsResultDto
{
    public required IEnumerable<OddsDto> LiveOdds { get; init; }
    public required IEnumerable<OddsDto> UpcomingOdds { get; init; }
}