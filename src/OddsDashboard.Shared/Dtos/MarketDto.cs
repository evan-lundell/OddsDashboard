namespace OddsDashboard.Shared.Dtos;

public record MarketDto
{
    public required string Key { get; init; }
    public DateTime LastUpdate { get; init; }
    public required IEnumerable<OutcomeDto> Outcomes { get; init; }
}